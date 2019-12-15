using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class ConflictException : ResponseException
    {
        public ConflictException(string name) : base($"{name} conflict", "", HttpStatusCode.Conflict) { }
    }
}