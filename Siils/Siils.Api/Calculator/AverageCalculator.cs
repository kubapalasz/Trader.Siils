using System.Collections.Generic;
using System.Linq;

namespace Siils.Api.Calculator
{
    public static class AverageCalculator
    {
        public static decimal SimpleMovingAverageCalculator(IEnumerable<decimal> prices)
        {
            var average = prices.Sum() / prices.Count();
            return average;
        }

        public static decimal WeightedMovingAverageCalculator(IEnumerable<decimal> prices)
        {
            decimal average = 0;
            var weight = 1;
            var totalWeights = 0;
            decimal totalPrices = 0;
            foreach (var price in prices)
            {
                totalPrices += weight * price;
                totalWeights += weight;
                weight += 1;
            }
            average = totalPrices / totalWeights / prices.Count();
            return average;
        }
    }
}
