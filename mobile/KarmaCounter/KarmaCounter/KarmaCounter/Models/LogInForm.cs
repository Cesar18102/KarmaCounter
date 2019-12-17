using System;
using System.Text;
using System.Security.Cryptography;

using Newtonsoft.Json;

using SolidityEncoder;

namespace KarmaCounter.Models
{
    public class LogInForm : IModelElement
    {
        private static readonly MD5 mD5 = MD5.Create();
        private static readonly KeccakEncoder encoder = new KeccakEncoder();

        [JsonProperty("login")]
        public string Login { get; private set; }

        [JsonProperty("pwd")]
        public string PasswordHash { get; private set; }

        [JsonProperty("seed")]
        public string Salt { get; private set; }

        public LogInForm(string login, string pwd)
        {
            Login = login;
            Salt = BitConverter.ToString(mD5.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()))).Replace("-", "").ToUpper();
            string pwdIntermediateHash = encoder.Encode(new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, pwd));
            PasswordHash = BitConverter.ToString(mD5.ComputeHash(Encoding.UTF8.GetBytes(pwdIntermediateHash + Salt))).Replace("-", "").ToUpper();
        }
    }
}
