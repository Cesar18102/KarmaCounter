using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class InvalidSessionException : ResponseException
    {
        public InvalidSessionException() : base("Session is invalid", "", HttpStatusCode.Unauthorized) { }
    }
}