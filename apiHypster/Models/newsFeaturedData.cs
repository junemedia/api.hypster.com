using Newtonsoft.Json;

namespace apiHypster.Models
{
    public class newsFeaturedData
    {
        public newsFeaturedData() { }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public content content { get; set; }
    }
}