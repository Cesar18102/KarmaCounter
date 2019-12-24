AP_CONFIG = { ssid = "esp8266", pwd = "11111111", auth = wifi.WPA2_PSK, max = 1 }
IP_CONFIG = { ip = "192.168.1.1", netmask = "255.255.255.252", gateway = "192.168.1.1" }
PORT_LISTENING = 80

DAY_MSEC = 60000 -- 86400000
MIN_WET_SECONDS_ALONG = 10 --1200
BRUSHES_PER_DAY = 2

print("start")

wifi.setmode(wifi.SOFTAP)
wifi.ap.config(AP_CONFIG)
wifi.ap.setip(IP_CONFIG)

print("access point active")

server = net.createServer(net.TCP)

print("server set up")

function credsReciever(conn, data)

    print(data)
    local creds = data:gmatch("ssid=.+")(0)
    
    if(creds == nil) then
        print("invalid creds recieved. rebooting.")
        conn:send("400")
        node.restart()
        return
    end

    ssid = creds:gmatch("ssid=([^&]+)")(0)
    pwd = creds:gmatch("pwd=(.+)")(0)

    if(pwd == nil) then pwd = "" end

    print("ssid parsed: "..ssid)
    print("pwd parsed: "..pwd)

    conn:send("200")
    conn:close()

    server:close()
    print("server closed")

    print("connecting to "..ssid)
    
    wifi.setmode(wifi.STATION)
    
    if(wifi.sta.config({ ssid = ssid, pwd = pwd })) then
        print("connected to "..ssid)
    else
        print("failed to connect to "..ssid.." wrong creds either ap is unreachable")
        node.restart()
        return
    end
    
    wifi.sta.connect()

    todayBrushes = 0
    wetSecondsAlong = 0

    dayTmr = tmr.create()
    dayTmr:register(DAY_MSEC, tmr.ALARM_AUTO, 
        function() 
            if(todayBrushes == BRUSHES_PER_DAY) then
                accountSuccess()
            else
                accountFail()
            end
            todayBrushes = 0
            wetSecondsAlong = 0
        end
    )
    dayTmr:start()

    checkTmr = tmr.create()
    checkTmr:register(1000, tmr.ALARM_AUTO, -- check wet every second
        function()
            if(checkWet()) then 
                wetSecondsAlong = wetSecondsAlong + 1
            else
                if(wetSecondsAlong >= MIN_WET_SECONDS_ALONG) then
                    todayBrushes = todayBrushes + 1
                end
                wetSecondsAlong = 0
            end
        end
    )
    checkTmr:start()
end

server:listen(PORT_LISTENING, 
    function(conn)
        conn:on("receive", credsReciever)
    end
)

print("server is listening to port "..PORT_LISTENING)
    

function accountSuccess()
    print("well done")
    -- TODO
end

function accountFail()
    print("fail")
    -- TODO
end

function checkWet()
    level = adc.read(0)
    if(level >= 128) then -- move to const and increase
        print(tmr.now()..": it's wet("..level..")")
        return true
    else
        print(tmr.now()..": it's dry("..level..")")
        return false
    end
end

