using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class UserConflictException : ResponseException
    {
        public UserConflictException() : base("User with such login already exists", HttpStatusCode.Conflict) { }
    }
}