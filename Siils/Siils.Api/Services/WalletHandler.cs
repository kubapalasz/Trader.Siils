using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Siils.Api.Infrastructure;
using Siils.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Siils.Api.Services
{
    public class WalletHandler
    {
        private string _balanceUrl = DebugMode.Enabled ? "http://localhost/SLYEx.API.Dummy/api/balance" : "http://localhost/SLYEx.API/balance";
        private string _tradingUrl = DebugMode.Enabled ? "http://localhost/SLYEx.API.Dummy/api/trading" : "http://localhost/SLYEx.API/trading";

        public void UpdateBalances()
        {
            using (var webClient = new WebClient())
            {
                dynamic balancesJson = webClient.DownloadString(_balanceUrl);
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

                //var balanceListString = jsSerializer.Deserialize<string>(balancesJson);
                var balanceList = jsSerializer.Deserialize<List<BalanceEntry>>(balancesJson);
                foreach (var balance in balanceList)
                {
                    int currency = (int) Enum.Parse(typeof (Currencies), balance.Currency);
                    Wallet.Balances[currency] = balance.Balance;
                }
            }
        }

        //{"source": EUR,"target":GBP,"namedPrice":0.8331,"amount":5000}
        public bool MakeTrade(string sourceCurrency, string targetCurrency, decimal price, decimal amountToBuy)
        {
            var trade = new Trade { SourceCurrency = sourceCurrency, TargetCurrency = targetCurrency, NamedPrice = price, AmountToBuy = (int)amountToBuy};
            var tradeJson = JsonConvert.SerializeObject(trade,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_tradingUrl);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(tradeJson);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                return httpResponse.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}