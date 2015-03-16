using System.Collections.Generic;
using System.Linq;
using Siils.Api.Models;

namespace Siils.Api.Calculator
{
    public class CrossingCheckCalculator : ILongTermRecommendationCalculator
    {
        private int shortNumber = 40;
        private int longNumber = 80;
        private bool IsGoldenCross(decimal previousShort, decimal currentShort, decimal previouslong, decimal currentlong)
        {
            return previousShort < previouslong && currentShort >= currentlong;
        }

        private bool IsDeathCross(decimal previousShort, decimal currentShort, decimal previouslong, decimal currentlong)
        {
            return previousShort > previouslong && currentShort <= currentlong;
        }

        public Recomendations GetRecommendation(IEnumerable<Price> prices)
        {
            var previousLong = AverageCalculator.SimpleMovingAverageCalculator(prices.Select(x=>x.Close).Skip(prices.Count() - longNumber-1).Take(longNumber));
            var currentLong = AverageCalculator.SimpleMovingAverageCalculator(prices.Select(x => x.Close).Skip(prices.Count() - longNumber));
            var previousShort = AverageCalculator.SimpleMovingAverageCalculator(prices.Select(x => x.Close).Skip(prices.Count() - shortNumber - 1).Take(shortNumber));
            var currentShort = AverageCalculator.SimpleMovingAverageCalculator(prices.Select(x => x.Close).Skip(prices.Count() - shortNumber));

            var isGoldenCross = IsGoldenCross(previousShort, currentShort, previousLong, currentLong);
            var isDeathCross = IsDeathCross(previousShort, currentShort, previousLong, currentLong);
            var recommendation = Recomendations.Wait;

            if (isGoldenCross)
                recommendation = Recomendations.Buy;
            if (isDeathCross)
                recommendation = Recomendations.Sell;

            return recommendation;
        }
    }
}
