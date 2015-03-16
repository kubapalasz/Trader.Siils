using System.Collections.Generic;
using System.Linq;
using Siils.Api.Models;

namespace Siils.Api.Calculator
{
    public class CandleStickCallculator : IShortTermRecommendationCalculator
    {
        private int _candleSize = 20;

        public Recomendations GetRecommendation(IEnumerable<Price> prices)
        {
            var firstCandle = CreateCandleStick(prices.Skip(prices.Count() - 3 * _candleSize).Take(_candleSize));
            var secondCandle = CreateCandleStick(prices.Skip(prices.Count() - 2 * _candleSize).Take(_candleSize));
            var lastCandle = CreateCandleStick(prices.Skip(prices.Count() - 1 * _candleSize).Take(_candleSize));

            var isBulishEngulfing = IsBulishEngulfing(firstCandle, secondCandle, lastCandle);
            var isHammer = IsHammer(firstCandle, secondCandle, lastCandle);
            var isBearEngulfing = IsBearEngulfing(firstCandle, secondCandle, lastCandle);

            var recommendation = Recomendations.Wait;
            if ((isBulishEngulfing || isHammer) && !isBearEngulfing)
            {
                recommendation = Recomendations.Buy;
            }
            if(!(isBulishEngulfing || isHammer) && isBearEngulfing)
            {
                recommendation = Recomendations.Sell;
            }
            return recommendation;
        }

        private bool IsBulishEngulfing(Price first, Price second, Price last)
        {
            var isFallingTrend = first.Open > first.Close && second.Open > second.Close;
            if (!isFallingTrend)
                return false;
            return last.Close > second.High && last.Open < second.Low;
        }

        private bool IsHammer(Price first, Price second, Price last)
        {
            var isFallingTrend = first.Open > first.Close && second.Open > second.Close;
            if (!isFallingTrend)
                return false;

            var isHammerShape = 3 * (second.Open - second.Close) <= second.Close - second.Low;

            return isHammerShape && last.Open < last.Close;

        }

        private bool IsBearEngulfing(Price first, Price second, Price last)
        {
            var isRisingTrend = first.Open < first.Close && second.Open < second.Close;
            if (!isRisingTrend)
                return false;
            return last.Close < second.Low && last.Open > second.High;
        }
        private Price CreateCandleStick(IEnumerable<Price> prices)
        {
            var candlestick = new Price { Close = prices.Last().Close, Open = prices.First().Open, High = prices.Max(x => x.High), Low = prices.Min(x => x.Low) };
            return candlestick;
        }

    }
}
