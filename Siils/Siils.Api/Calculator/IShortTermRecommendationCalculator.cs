using System.Collections.Generic;
using Siils.Api.Models;

namespace Siils.Api.Calculator
{
    public interface IShortTermRecommendationCalculator
    {
        Recomendations GetRecommendation(IEnumerable<Price> prices);
    }
}
