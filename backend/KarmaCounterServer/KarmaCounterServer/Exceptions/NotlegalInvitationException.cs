using System.Net;

namespace KarmaCounterServer.Exceptions
{
    public class NotLegalInvitationException : ResponseException
    {
        public NotLegalInvitationException() : base("You're not a member or creator of the group", "", HttpStatusCode.Forbidden) { }
    }
}