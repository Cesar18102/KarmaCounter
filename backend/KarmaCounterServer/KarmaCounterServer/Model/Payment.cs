using System.ComponentModel.DataAnnotations;

namespace KarmaCounterServer.Model
{
    public class Payment : IModelElement
    {
        [Required]
        public string data { get; set; }

        [Required]
        public string signature { get; set; }

        public Payment() { }

        public Payment(string data, string signature)
        {
            this.data = data;
            this.signature = signature;
        }
    }
}