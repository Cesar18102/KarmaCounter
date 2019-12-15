using System.Net;

using Newtonsoft.Json;

namespace KarmaCounterServer.Exceptions
{
    public class PaymentNeededException : ResponseException
    {
        public PaymentNeededException(int amount) : base("Payment required", JsonConvert.DeserializeObject("{ \"amount\" : " + amount + " }"), HttpStatusCode.PaymentRequired) { }
    }
}