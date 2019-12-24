-- wifi.getmode() == wifi.STATION
-- wifi.getphymode() == wifi.PHYMODE_N

PORT = 80
STA_MAX = 4
STA_COUNT = 0
STA_LIGHT = { }
NULL_MAC = "00:00:00:00:00:00"
AP_CONFIG = { ssid = "esp8266", pwd = "56552239", auth = wifi.WPA2_PSK }
IP_CONFIG = { ip = "192.168.1.1", netmask = "255.255.255.0", gateway = "192.168.1.1" }

CONNECTION = false
SERVER = net.createServer(net.TCP, 60)



function IndexOf(array, value)
    for k, v in pairs(array) do
        if v == value then
            return k
        end
    end
end

function MacToIP(MAC) 
    for mac, ip in pairs(wifi.ap.getclient()) do
        if(mac == MAC) then
            return ip
        end
    end
end

function IPToMac(IP)
    for mac, ip in pairs(wifi.ap.getclient()) do
        if(ip == IP) then
            return mac
        end
    end
end

function CreateConnection(server, recieve_callback)
    server:listen(PORT, 
        function(conn) 
            conn:on("receive", recieve_callback)
        end
    )
end

-------------------------------------------------

for i = 1, STA_MAX do
    gpio.mode(i, gpio.OUTPUT)
    gpio.write(i, gpio.LOW)
    STA_LIGHT[i] = NULL_MAC
end

wifi.setmode(wifi.SOFTAP)
wifi.ap.config(AP_CONFIG)
wifi.ap.setip(IP_CONFIG)

--tmr.create():alarm(1000, tmr.ALARM_AUTO, function() print(gpio.read(1)) end)-

wifi.eventmon.register(wifi.eventmon.AP_STACONNECTED, 
    function(T)
        for i = 1, STA_MAX do
            if STA_LIGHT[i] == NULL_MAC then
                STA_LIGHT[i] = T.MAC
                print("MAC="..STA_LIGHT[i].." connected")
                gpio.write(i, gpio.HIGH)          
                STA_COUNT = STA_COUNT + 1    

                if(not CONNECTION) then
                    tmr.create():alarm(1000, tmr.ALARM_SINGLE, 
                        function() 
                            CreateConnection(SERVER, 
                                function(conn, data) 
                                    print("req: "..data)
        
                                    _,ip = conn:getpeer()
                                    mac = IPToMac(ip)
                                    light_num = IndexOf(STA_LIGHT, mac)
                                    gpio.write(light_num, gpio.LOW)
                                    tmr.create():alarm(100, tmr.ALARM_SINGLE, function() gpio.write(light_num, gpio.HIGH) end)
                                    
                                    conn:send("<div>Fuck you</div>")
                                    --conn:close()
                                end
                            )
                        end
                    )
                    CONNECTION = true
                end
                break
            end
        end
    end
)

wifi.eventmon.register(wifi.eventmon.AP_STADISCONNECTED, 
    function(T)
        for i = 1, STA_MAX do
            if STA_LIGHT[i] == T.MAC then
                print(STA_LIGHT[i].." disconnected")
                STA_LIGHT[i] = NULL_MAC
                gpio.write(i, gpio.LOW)
                STA_COUNT = STA_COUNT - 1
                break
            end
        end
        if(STA_COUNT == 0 and CONNECTION) then
            SERVER:close()
            CONNECTION = false
        end
    end
)
    
