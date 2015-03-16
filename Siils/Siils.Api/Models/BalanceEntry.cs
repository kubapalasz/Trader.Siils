using System;

namespace Siils.Api.Models
{
    public class BalanceEntry
    {
        public DateTime TimeStamp { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
    }
}