using System;

using Android.Runtime;

using Newtonsoft.Json;

namespace KarmaCounter.Server.Output
{
    [Preserve(AllMembers = true)]
    [JsonObject(MemberSerialization.OptOut)]
    public class ResponseException : Exception
    {
        [JsonRequired]
        public string message { get; set; }

        [JsonRequired]
        public string body { get; set; }

        public ResponseException() { }

        public ResponseException(string message) => this.message = message;

        public override string ToString() => "{ \"message\" : \"" + message + "\", \"body\" : \"" + body + "\" }";
    }
}
