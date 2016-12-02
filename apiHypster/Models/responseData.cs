using Newtonsoft.Json;

namespace apiHypster.Models
{
    public class responseData
    {
        public responseData() { }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public memberUser user { get; set; }
    }    
}