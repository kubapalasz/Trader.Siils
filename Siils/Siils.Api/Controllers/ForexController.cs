using Siils.Api.Calculator;
using Siils.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Siils.Api.Controllers
{
    public class ForexController : ApiController
    {
        private DecisionService _decisionService;

        public ForexController()
        {
            _decisionService = new DecisionService();
        }

        // POST api/values
        public void Post([FromBody] List<Price> currencies)
        {
            try
            {


                foreach (var value in currencies)
                {
                    if (value.Source == Currencies.GBP || value.Target == Currencies.GBP)
                    {
                        if (value.Target == Currencies.GBP)
                        {
                            value.Target = value.Source;
                            value.Source = Currencies.GBP;
                            value.Open = 1/value.Open;
                            value.Close = 1/value.Close;
                            value.High = 1/value.High;
                            value.Low = 1/value.Low;
                        }
                        PricesList.ConcurrentPrices.AddLast(value);
                    }
                }

                if (PricesList.ConcurrentPrices.Count > 600)
                {
                    _decisionService.Process(PricesList.ConcurrentPrices.Where(p => p.Target == Currencies.EUR));
                    _decisionService.Process(PricesList.ConcurrentPrices.Where(p => p.Target == Currencies.USD));
                    _decisionService.Process(PricesList.ConcurrentPrices.Where(p => p.Target == Currencies.JPY));
                }

                if (PricesList.ConcurrentPrices.Count > 1000)
                    for (int i = 0; i < 200; i++)
                    {
                        PricesList.ConcurrentPrices.RemoveFirst();
                    }
            }
            catch
            {
                Console.WriteLine("1 Million GBP should be enough :)");
            }
        }
    }
}

