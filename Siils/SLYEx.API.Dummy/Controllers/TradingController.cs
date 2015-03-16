using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Siils.Api.Models;

namespace SLYEx.API.Dummy.Controllers
{
    public class TradingController : ApiController
    {
        private Dictionary<string, Currencies> Map;

        public TradingController()
        {
            Map = new Dictionary<string, Currencies>
            {
                {"EUR", Currencies.EUR},
                {"GBP", Currencies.GBP},
                {"JPY", Currencies.JPY},
                {"USD", Currencies.USD}
            };
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]Trade trade)
        {
            var currentPrices = PricesContainer.Current;
            var currentBalance = BalanceContainer.Current;

            var exchangeRate = GetExchangeRate(currentPrices, trade.SourceCurrency, trade.TargetCurrency);
            if (trade.NamedPrice < exchangeRate)
            {
                // Named rate is invalid
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            // check if you have enough money in source currency
            var sourceCurrencyBalance = currentBalance.Single(_ => _.Currency == trade.SourceCurrency).Balance;
            var currencyToBy = trade.AmountToBuy*(1/trade.NamedPrice);
            if (sourceCurrencyBalance < currencyToBy)
            {
                // not enough money
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            var source = BalanceContainer.Current.Single(_ => _.Currency == trade.SourceCurrency);
            var target = BalanceContainer.Current.Single(_ => _.Currency == trade.TargetCurrency);
            source.Balance -= currencyToBy;
            source.TimeStamp = DateTime.Now;
            target.Balance += trade.AmountToBuy;
            target.TimeStamp = DateTime.Now;

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        private decimal GetExchangeRate(IEnumerable<Price> currentPrices, string sourceCurrency, string targetCurrency)
        {
            foreach (var currentPrice in currentPrices)
            {
                if (currentPrice.Source == Map[sourceCurrency] && currentPrice.Target == Map[targetCurrency])
                {
                    return currentPrice.Close;
                }

                if (currentPrice.Source == Map[targetCurrency] && currentPrice.Target == Map[sourceCurrency])
                {
                    return 1 / currentPrice.Close;
                }
            }

            throw new System.NotImplementedException("Did not find exchange rate");
        }
    }
}
