using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class ForbiddenException : ResponseException
    {
        public ForbiddenException() : base("Wrong password", HttpStatusCode.Forbidden) { }
    }
}