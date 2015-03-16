using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Siils.Api.Infrastructure;
using Siils.Api.Models;

namespace SLYEx.API.Dummy
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           
            BalanceContainer.Current = new List<BalanceEntry>
            {
                new BalanceEntry
                {
                    Balance = 1000000,
                    Currency = "GBP",
                    TimeStamp = DateTime.Now
                },
                new BalanceEntry
                {
                    Balance = 0,
                    Currency = "EUR",
                    TimeStamp = DateTime.Now
                },
                new BalanceEntry
                {
                    Balance = 0,
                    Currency = "USD",
                    TimeStamp = DateTime.Now
                },
                new BalanceEntry
                {
                    Balance = 0,
                    Currency = "JPY",
                    TimeStamp = DateTime.Now
                }
            };

            if (DebugMode.Enabled)
            {
                Task.Factory.StartNew(() =>
                {
                    var currentDate = DateTime.Parse("2012-01-02 06:01:00");
                    var endDate = DateTime.Parse("2013-12-31 19:01:00");
                    var repo = new SampleRepository();
                    const double stepInMinutes = 5;

                    do
                    {
                        var prices = repo.GetPrices(currentDate);
                        PricesContainer.Current = prices;
                        currentDate = currentDate.AddMinutes(stepInMinutes);
                        System.Threading.Thread.Sleep(1000);

                        try
                        {
                            NotifyPricesChanged(prices);
                        }
                        catch (Exception ex)
                        {
                        }

                    } while (currentDate < endDate);
                });
            }
        }

        public bool NotifyPricesChanged(List<Price> current)
        {
            var currentBalanceJson = JsonConvert.SerializeObject(current,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost/Siils.Api/api/forex");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(currentBalanceJson);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                return httpResponse.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}
