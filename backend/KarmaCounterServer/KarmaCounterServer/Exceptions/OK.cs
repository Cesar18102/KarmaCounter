using System.Net;
using System.Web.Http;

namespace KarmaCounterServer.Exceptions
{
    public class OK : ResponseException
    {
        public OK() : base("", "", HttpStatusCode.OK) { }
    }
}