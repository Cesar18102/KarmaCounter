using System.Net;
using System.Collections.Generic;

namespace KarmaCounter.Server.Output
{
    public interface IServerResponse
    {
        string Data { get; }
        IDictionary<string, string> Headers { get; }
        CookieContainer CookieContainer { get; }
    }
}
