using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Siils.Api.Models;
using Siils.Api.Services;

namespace Siils.Api.Calculator
{
    public class DecisionService
    {

        private readonly IShortTermRecommendationCalculator _shortCalculator;
        private readonly ILongTermRecommendationCalculator _longCalculator;
        private readonly WalletHandler _walletHandler;

        public DecisionService() : this( new CandleStickCallculator(), new CrossingCheckCalculator() )
        {
                
        }

        public DecisionService(IShortTermRecommendationCalculator shortCalculator,
            ILongTermRecommendationCalculator longCalculator)
        {
            _shortCalculator = shortCalculator;
            _longCalculator = longCalculator;
            _walletHandler = new WalletHandler();
        }

        public void Process(IEnumerable<Price> prices)
        {
            var magicNumberSpread = 0.03M;
            var useShortTerm = true;
            var lastPrice = prices.Last();
            var longRecommendation = _longCalculator.GetRecommendation(prices);
            if (longRecommendation == Recomendations.Buy)
            {
                //Buy logic here
                useShortTerm = false;
                _walletHandler.UpdateBalances();
                _walletHandler.MakeTrade(lastPrice.Source.ToString(), lastPrice.Target.ToString(),
                    lastPrice.Close + magicNumberSpread, Wallet.Balances[(int)Currencies.GBP] * 0.3M);
            }
            if (longRecommendation == Recomendations.Sell)
            {
                //Sell logic here
                useShortTerm = false;
                _walletHandler.UpdateBalances();
                _walletHandler.MakeTrade(lastPrice.Target.ToString(), lastPrice.Source.ToString(),
                    lastPrice.Close + magicNumberSpread, Wallet.Balances[(int)lastPrice.Target] * 0.3M);
            }

            if (useShortTerm)
            {
                var shortRecommendation = _shortCalculator.GetRecommendation(prices);
                if (shortRecommendation == Recomendations.Buy)
                {
                    //Buy logic here
                    _walletHandler.UpdateBalances();
                    _walletHandler.MakeTrade(lastPrice.Source.ToString(), lastPrice.Target.ToString(),
                        lastPrice.Close + magicNumberSpread, Wallet.Balances[(int) Currencies.GBP]*0.15M);
                }

                if (shortRecommendation == Recomendations.Sell)
                {
                    //Sell logic here
                    _walletHandler.UpdateBalances();
                    _walletHandler.MakeTrade(lastPrice.Target.ToString(), lastPrice.Source.ToString(),
                        lastPrice.Close + magicNumberSpread, Wallet.Balances[(int) lastPrice.Target]*0.15M);
                }

            }
            
        }

    }
}
