using System.Net;
using System.Threading.Tasks;

using KarmaCounter.Server.Input;
using KarmaCounter.Server.Output;

namespace KarmaCounter.Server.Access
{
    public interface IServerCommunicator
    {
        string ServerURL { get; set; }
        Task<IServerResponse> SendQuery(IQuery query, CookieContainer container = null);
    }
}
