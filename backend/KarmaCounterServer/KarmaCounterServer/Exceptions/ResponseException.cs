using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json;

namespace KarmaCounterServer.Exceptions
{
    [JsonObject]
    public class ResponseException : HttpResponseException
    {
        public ResponseException(string message, object body, HttpStatusCode status) :
            base(new HttpResponseMessage(status) { Content = new StringContent("{ \"message\" : \"" + message + "\", \"body\" : " + JsonConvert.SerializeObject(body) + " }") }) { }

        public ResponseException(Exception ex, object body, HttpStatusCode status) :
            base(new HttpResponseMessage(status) { Content = new StringContent("{ \"message\" : \"" + ex.Message + "\", \"body\" : " + JsonConvert.SerializeObject(body).Trim('"') + " }") }) { }
    }
}