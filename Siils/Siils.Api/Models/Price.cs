using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Siils.Api.Controllers;

namespace Siils.Api.Models
{
    public class Price
    {
        [JsonProperty("timeStamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("source")]
        public Currencies Source { get; set; }
        [JsonProperty("target")]
        public Currencies Target { get; set; }
        [JsonProperty("open")]
        public decimal Open { get; set; }
        [JsonProperty("close")]
        public decimal Close { get; set; }
        [JsonProperty("high")]
        public decimal High { get; set; }
        [JsonProperty("low")]
        public decimal Low { get; set; }
    }

}