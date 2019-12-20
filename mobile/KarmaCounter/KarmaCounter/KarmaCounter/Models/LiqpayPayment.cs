using System;
using System.Text;
using System.Security.Cryptography;

using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiqpayPayment : IModelElement
    {
        private const string PUBLIC_KEY = "sandbox_i74310151520";
        private const string PRIVATE_KEY = "sandbox_kgwzzF9TsmUOIJmQQeQyM4G4yrxfGJxVq64k8hLn";

        private const int VERSION = 3;
        private const string ACTION = "pay";
        private const string CURRENCY = "USD";

        private static readonly MD5 mD5 = MD5.Create();
        private static readonly SHA1 Sha1 = SHA1.Create();

        [JsonProperty("action")]
        private string Action { get; set; }

        [JsonProperty("version")]
        private int Version { get; set; }

        [JsonProperty("public_key")]
        private string PublicKey { get; set; }

        [JsonProperty("amount")]
        private double Amount { get; set; }

        [JsonProperty("currency")]
        private string Currency { get; set; }

        [JsonProperty("description")]
        private string Description { get; set; }

        [JsonProperty("order_id")]
        private string OrderId { get; set; }

        [JsonProperty("server_url")]
        private string ServerUrl { get; set; }

        [JsonProperty("info")]
        private string Info { get; set; }

        [JsonIgnore]
        public string Base64 { get; private set; }

        [JsonIgnore]
        public string Signature { get; private set; }

        public LiqpayPayment(string paymentGroupInfo, string groupName, double amount, string serverCallback)
        {
            Action = ACTION;
            Version = VERSION;
            PublicKey = PUBLIC_KEY;
            Currency = CURRENCY;

            Amount = amount;
            Info = paymentGroupInfo;
            ServerUrl = serverCallback;
            Description = $"Creating group \"{groupName}\"";
            OrderId = BitConverter.ToString(mD5.ComputeHash(Encoding.UTF8.GetBytes(groupName + DateTime.Now.Ticks))).Replace("-", "").ToUpper();

            string data = JsonConvert.SerializeObject(this);
            Base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
            Signature = Convert.ToBase64String(Sha1.ComputeHash(Encoding.UTF8.GetBytes(PRIVATE_KEY + Base64 + PRIVATE_KEY)));
        }
    }
}
