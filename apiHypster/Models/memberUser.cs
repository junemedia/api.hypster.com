using Newtonsoft.Json;

namespace apiHypster.Models
{
    public class memberUser
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        public int adminlevel { get; set; }
    }
}