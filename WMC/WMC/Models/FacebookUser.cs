using Newtonsoft.Json;

namespace WMC.Models
{
    [JsonObject]
    public class FacebookUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}