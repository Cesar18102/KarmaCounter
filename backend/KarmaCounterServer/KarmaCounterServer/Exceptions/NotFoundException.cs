using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class NotFoundException : ResponseException
    {
        public NotFoundException(string obj) : base($"{obj} not found", "", HttpStatusCode.NotFound) { }
    }
}