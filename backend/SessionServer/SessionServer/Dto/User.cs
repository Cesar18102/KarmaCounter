using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace SessionServer.Dto
{
    public class User
    {
        [Required]
        [JsonRequired]
        [JsonProperty("userId")]
        public long Id { get; private set; }
    }
}