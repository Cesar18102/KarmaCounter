using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using KarmaCounterServer.Model;
using KarmaCounterServer.Exceptions;

namespace KarmaCounterServer.Services
{
    public class SessionService
    {
        private const string SESSION_SERVER_ADDRESS = "http://karmasessionserver.somee.com/";
        private const string SESSION_SERVER_CREATE = SESSION_SERVER_ADDRESS + "api/session/create";
        private const string SESSION_SERVER_CHECK = SESSION_SERVER_ADDRESS + "api/session/check";

        private const string APP_JSON_CONTENT_TYPE = "application/json";
        private const string USER_ID_PARAM_NAME = "userId";

        public async Task<Session> CreateSession(string userId)
        {
            string data = "{ \"" + USER_ID_PARAM_NAME + "\" : \"" + userId + "\" }";
            string responseText = "";

            using (HttpWebResponse response = await SendQuery(WebRequestMethods.Http.Post, APP_JSON_CONTENT_TYPE, SESSION_SERVER_CREATE, data))
            using (StreamReader str = new StreamReader(response.GetResponseStream()))
                responseText = str.ReadToEnd();

            try { return JsonConvert.DeserializeObject<Session>(responseText); }
            catch { throw JsonConvert.DeserializeObject<BadRequestException>(responseText); }
        }

        public async Task<BoolResult> CheckSession(Session session)
        {
            using (HttpWebResponse response = await SendQuery(WebRequestMethods.Http.Post, APP_JSON_CONTENT_TYPE,
                                                              SESSION_SERVER_CHECK, JsonConvert.SerializeObject(session)))
            using (StreamReader str = new StreamReader(response.GetResponseStream()))
                return JsonConvert.DeserializeObject<BoolResult>(str.ReadToEnd());
        }

        public async Task<HttpWebResponse> SendQuery(string method, string contentType, string url, string data)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = method;
            request.ContentType = contentType;

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            request.ContentLength = dataBytes.Length;

            using (Stream str = await request.GetRequestStreamAsync())
                str.Write(dataBytes, 0, dataBytes.Length);

            return (await request.GetResponseAsync()) as HttpWebResponse;
        }
    }
}