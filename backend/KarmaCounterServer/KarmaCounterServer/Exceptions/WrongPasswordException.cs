using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class WrongPasswordException : ResponseException
    {
        public WrongPasswordException() : base("Wrong password", "", HttpStatusCode.Forbidden) { }
    }
}