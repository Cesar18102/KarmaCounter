using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Autofac;

using KarmaCounter.Models;
using KarmaCounter.ResponseParser;
using KarmaCounter.Server.Access;
using KarmaCounter.Server.Input;
using KarmaCounter.Server.Output;

namespace KarmaCounter.Server.Interaction
{
    public class LogicServerWrapper
    {
        private const int RESEND_DELAY = 1000;
        private const int NOT_RESPONDING_MAX_COUNT = 10;
        private static readonly Regex NOT_RESPONDING_ANSWER_PATTERN = new Regex("(<!DOCTYPE HTML)|(<!DOCTYPE html)");
        private static readonly Regex ERROR_RESPONSE_ANSWER_PATTERN = new Regex("(An error has occurred)");

        public string CurrentConnection => Server?.ServerURL;

        private IServerCommunicator Server { get; set; }
        private int NotRespondingCount = 0;

        public async Task<IServerResponse> SendQuery(IQuery query, bool repeat, CookieContainer container = null)
        {
            if (Server == null || string.IsNullOrEmpty(Server.ServerURL) || NotRespondingCount >= NOT_RESPONDING_MAX_COUNT)
            {
                await Reconnect();
                return await SendQuery(query, repeat, container);
            }

            try
            {
                IServerResponse response = await Server.SendQuery(query, container);

                if (NOT_RESPONDING_ANSWER_PATTERN.IsMatch(response.Data))
                {
                    NotRespondingCount++;
                    Thread.Sleep(RESEND_DELAY);
                    return await SendQuery(query, repeat, container);
                } 
                else if(ERROR_RESPONSE_ANSWER_PATTERN.IsMatch(response.Data))
                {
                    NotRespondingCount++;
                    if (repeat)
                    {
                        Thread.Sleep(RESEND_DELAY);
                        return await SendQuery(query, repeat, container);
                    }
                    else
                        return null;
                }

                return response;
            }
            catch (ResponseException ex) { NotRespondingCount++; throw ex; }
        }

        public async Task Reconnect()
        {
            ServerInfo serverInfo = await GetBestServerInfo();
            NotRespondingCount = 0;

            if (Server == null)
                Server = new ServerCommunicator(serverInfo.Url + "/api/");
            else
                Server.ServerURL = serverInfo.Url + "/api/";
        }

        private const string LOAD_BALANCER_URL = "http://karmaloadbalancer.somee.com/api/";
        private const string GET_LOGIC_SERVER_IP_SERVER_METHOD_NAME = "server/get";
        private static readonly ServerCommunicator LOAD_BALANCER_SERVER = new ServerCommunicator(LOAD_BALANCER_URL);

        public async Task<ServerInfo> GetBestServerInfo()
        {
            IQuery getServerInfoQuery = new Query(QueryMethod.GET, GET_LOGIC_SERVER_IP_SERVER_METHOD_NAME);
            IServerResponse response = await LOAD_BALANCER_SERVER.SendQuery(getServerInfoQuery);

            if (NOT_RESPONDING_ANSWER_PATTERN.IsMatch(response.Data))
                return await GetBestServerInfo();

            return DI.Services.Resolve<IResponseParser>().Parse<ServerInfo, ResponseException>(response);
        }
    }
}
