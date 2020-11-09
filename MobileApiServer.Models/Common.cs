using Newtonsoft.Json;

namespace MobileApiServer.Models
{
    public class InnerResult
    {
        /// <summary></summary>
        /// <example>true</example>
        [JsonProperty("success")]
        public bool Success { get; set; } = true;

        /// <summary></summary>
        /// <example>0</example>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary></summary>
        /// <example>null</example>
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; set; }
    }
}
