using System;
using System.Linq;
using System.Text;
using System.Numerics;

using Nethereum.Util;

namespace SolidityEncoder
{
    public class KeccakEncoder
    {
        public enum HashType
        {
            String,
            Address,
            UInt256
        };

        public class ToBeHashed
        {
            public HashType T { get; private set; }
            public string V { get; private set; }

            public ToBeHashed(HashType T, string V)
            {
                this.T = T;
                this.V = V;
            }
        }

        private const string HEX_PREFIX = "0x";
        private Sha3Keccack keccack256 = new Sha3Keccack();

        public string Encode(params ToBeHashed[] args)
        {
            string toHash = HEX_PREFIX;
            foreach (ToBeHashed arg in args)
                switch (arg.T)
                {
                    case HashType.Address: toHash += arg.V.Replace("0x", ""); break;
                    case HashType.String: toHash += BitConverter.ToString(Encoding.ASCII.GetBytes(arg.V)).Replace("-", ""); break;
                    case HashType.UInt256:
                        byte[] bytes = BigInteger.Parse(arg.V).ToByteArray().Reverse().ToArray();
                        string num = BitConverter.ToString(bytes).Replace("-", "");
                        toHash += new string('0', 64 - num.Length) + num;
                        break;
                }

            return keccack256.CalculateHashFromHex(toHash.ToLower());
        }
    }
}
