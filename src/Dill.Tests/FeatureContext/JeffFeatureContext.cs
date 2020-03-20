using Dill.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Relish.Tests.Examples;

namespace Dill.Tests.FeatureContext
{
    public class JeffFeatureContext : global::Dill.FeatureContext
    {
        private readonly Jeff _jeff;
        private decimal _startBalance = 0.00m;
        private decimal _microwavePurchasePrice = 0.00m;

        public JeffFeatureContext(Dill dill) : base(dill, "jeff.feature")
        {
            _jeff = new Jeff();
        }

        [Given(@"^Jeff has bought a microwave for \$(\d+)$")]
        public void Given_JeffBuysMicrowave(decimal purchasePrice)
        {
            _startBalance = _jeff.Money;
            _microwavePurchasePrice = purchasePrice;
            _jeff.BuyFaultyMicrowave(purchasePrice);
        }

        [And("he has a receipt")]
        public void And_HeHasReceipt()
        {
            _jeff.HasReceipt = true;
        }

        [And("he forgot his receipt")]
        public void And_HeForgotReceipt()
        {
            _jeff.HasReceipt = false;
        }

        [When("he returns the microwave")]
        public void When_HeReturnsMicrowave()
        {
            _jeff.ReturnMicrowave();
        }

        [Then("Jeff should be refunded \\$(\\d+)")]
        public void Then_ShouldBeRefunded(decimal refundPrice)
        {
            Assert.AreEqual(_startBalance - _microwavePurchasePrice + refundPrice,_jeff.Money);
        }

        [And("Jeff should (not have|have) a microwave")]
        public void And_JeffMaybeHasAMicrowave(string hasMicrowaveString)
        {
            if (hasMicrowaveString == "not have")
            {
                Assert.IsFalse(_jeff.HasFaultyMicrowave);
            }

            if (hasMicrowaveString == "have")
            {
                Assert.IsTrue(_jeff.HasFaultyMicrowave);
            }
        }
    }
}
