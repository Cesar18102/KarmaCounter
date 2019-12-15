using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class ForbiddenException : ResponseException
    {
        public ForbiddenException(string message) : base(message, "", HttpStatusCode.Forbidden) { }
    }
}