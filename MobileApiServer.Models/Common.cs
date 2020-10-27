using Newtonsoft.Json;

namespace MobileApiServer.Models
{
    public class InnerResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; } = true;
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; set; }
    }
}
