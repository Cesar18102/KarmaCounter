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
        public ResponseException(string message, HttpStatusCode status) :
            base(new HttpResponseMessage(status) { Content = new StringContent("{ \"message\" : \"" + message + "\" }") }) { }

        public ResponseException(Exception ex, HttpStatusCode status) :
            base(new HttpResponseMessage(status) { Content = new StringContent("{ \"message\" : \"" + ex.Message + "\" }") }) { }
    }
}