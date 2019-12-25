AP_CONFIG = { ssid = "esp8266", pwd = "11111111", auth = wifi.WPA2_PSK, max = 1 }
IP_CONFIG = { ip = "192.168.1.1", netmask = "255.255.255.252", gateway = "192.168.1.1" }
PORT_LISTENING = 80

ADHERENCE_ENDPOINT = "/api/group/accountruleadherence"
VIOLATION_ENDPOINT = "/api/group/accountruleviolation"
BALANCER_URL = "http://karmaloadbalancer.somee.com/api/server/get/"

PUBLIC_KEY = "a98235230a71b5eef4064cae6b05e67bfc1ff4f5943ad09fe93103ed62634ab7"
PRIVATE_KEY = "271793bcd2660d3763a043fa45f0180e696c0b7e4c3408c109cf6d7ff5137c7b"
USER_ID = 127
RULE_ID = 12

MIN_WET = 512
DAY_MSEC = 20000 -- 86400000
MIN_WET_SECONDS_ALONG = 10 --1200
BRUSHES_PER_DAY = 2

function try(f, catch_f)
    local status, exception = pcall(f)
    if not status then
        catch_f(exception)
    end
end

function tryRequest(url, method, headers, body, checkOkCallback, cycle, delay)
    http.request(url, method, headers, body, 
        function(code, data)
            if(not checkOkCallback(code, data) and cycle) then
                tmr.delay(delay)
                tryRequest(url, method, headers, body, checkOkCallback, cycle, delay)
            end
        end
    )
end

function getServer(callback)
    tryRequest(BALANCER_URL, "GET", "", "", 
        function(code, data)
            if(code == 200) then
                callback(data)
                return true
            end
            return false
        end, true, 1000
    )
end

function createActionAccountTable(points)

    table = { 
        rule_id = RULE_ID, 
        user_id = USER_ID, 
        public_key = PUBLIC_KEY,
        hash = string.upper(crypto.toHex(crypto.hash("md5", PRIVATE_KEY.."_"..RULE_ID.."_"..USER_ID.."_"..PUBLIC_KEY.."_"..PRIVATE_KEY))),
        values = { points }
    }

    return table

end


function accountSuccess(brushes, surl)
    print("seding adherence to "..surl)

    jsonData = sjson.encode(createActionAccountTable(brushes))
    print("sending "..jsonData)
    
    tryRequest(surl..ADHERENCE_ENDPOINT, "POST", "Content-Type: application/json\r\n", jsonData,
        function(data)
            print(data.." is responsed for sending adherence to "..surl)
        end, false, 0
    )
end

function accountFail(brushes, surl)
    print("seding fail to "..surl)

    jsonData = sjson.encode(createActionAccountTable(math.abs(BRUSHES_PER_DAY - brushes)))
    print("sending "..jsonData)
    
    tryRequest(surl..VIOLATION_ENDPOINT, "POST", "Content-Type: application/json\r\n", jsonData,
        function(data)
            print(data.." is responsed for sending violation to "..surl)
        end, false, 0
    )
end

function checkWet()
    level = adc.read(0)
    if(level >= MIN_WET) then
        print(tmr.now()..": it's wet("..level..")")
        return true
    else
        print(tmr.now()..": it's dry("..level..")")
        return false
    end
end

function startCheckingAdherence()

    serv = {}
    getServer(
        function(data)
            serv = sjson.decode(data)
            print(serv.domain)
        end
    )
    
    todayBrushes = 0
    wetSecondsAlong = 0

    dayTmr = tmr.create()
    dayTmr:register(DAY_MSEC, tmr.ALARM_AUTO, 
        function() 
            if(wetSecondsAlong >= MIN_WET_SECONDS_ALONG) then
                todayBrushes = todayBrushes + 1 -- if day is out, but it's still wet
            end
            if(todayBrushes == BRUSHES_PER_DAY) then
                accountSuccess(todayBrushes, serv.domain)
            else
                accountFail(todayBrushes, serv.domain)
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

function connectAP(ssid, pwd)

    print("connecting to "..ssid)
    wifi.setmode(wifi.STATION)

    sta_cnf = { ssid = ssid, pwd = pwd, auto = false }

    print(sta_cnf.ssid)
    print(sta_cnf.pwd)
    
    if(wifi.sta.config(sta_cnf)) then
        print("connected to "..ssid)
    else
        print("failed to connect to "..ssid.." wrong creds either ap is unreachable")
        node.restart()
        return
    end

    wifi.sta.connect(
        function(e)
            startCheckingAdherence()
        end
    )
end

function credsReciever(conn, data)

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
    
    connectAP(ssid, pwd)
end

--------------------------------------------------

print("start")

wifi.setmode(wifi.SOFTAP)
wifi.ap.config(AP_CONFIG)
wifi.ap.setip(IP_CONFIG)

print("access point active")

server = net.createServer(net.TCP)

print("server set up")

server:listen(PORT_LISTENING, 
    function(conn)
        conn:on("receive", credsReciever)
    end
)

print("server is listening to port "..PORT_LISTENING)