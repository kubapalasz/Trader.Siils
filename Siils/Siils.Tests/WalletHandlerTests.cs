using Microsoft.VisualStudio.TestTools.UnitTesting;
using Siils.Api.Services;

namespace Siils.Tests
{
    [TestClass]
    public class WalletHandlerTests
    {
        [TestMethod]
        public void SuccessfullyMakesTrade()
        {
            var walletHandler = new WalletHandler();
            var tradeAccepted = walletHandler.MakeTrade("EUR", "GBP", 10M, 100M);
        }
    }
}
