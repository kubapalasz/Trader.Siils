using Newtonsoft.Json;

namespace Siils.Api.Models
{
    public class Trade
    {
        [JsonProperty("source")]
        public string SourceCurrency { get; set; }
        [JsonProperty("target")]
        public string TargetCurrency { get; set; }
        [JsonProperty("namedPrice")]
        public decimal NamedPrice { get; set; }
        [JsonProperty("amount")]
        public int AmountToBuy { get; set; }
    }
}