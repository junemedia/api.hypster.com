using Newtonsoft.Json;

namespace apiHypster.Models
{
    public class responseData
    {
        public responseData() { }
        public string status { get; set; }
        public string message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public memberUser user { get; set; }
    }    
}