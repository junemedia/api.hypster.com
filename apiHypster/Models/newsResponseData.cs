using Newtonsoft.Json;

namespace apiHypster.Models
{
    public class newsResponseData
    {

        public newsResponseData() { }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public newsPosts[] content { get; set; }
    }
}