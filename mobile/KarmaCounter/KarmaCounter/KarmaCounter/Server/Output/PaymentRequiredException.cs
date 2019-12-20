using System;

using Newtonsoft.Json;

namespace KarmaCounter.Server.Output
{
    public class PaymentRequiredException : Exception
    {
        [JsonRequired]
        [JsonProperty("amount")]
        public double Amount { get; private set; }

        public PaymentRequiredException(double amount) => Amount = amount;
    }
}
