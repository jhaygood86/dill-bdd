using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dill.Attributes;
using Dill.Tests.Examples;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dill.Tests.FeatureContext
{
    public class RomanNumeralFeatureContext : global::Dill.FeatureContext
    {
        private RomanNumeralCalculator _calculator;

        public RomanNumeralFeatureContext(Dill dill) : base(dill, "roman_numeral.feature")
        {
            _calculator = new RomanNumeralCalculator();
        }

        [Given("I enter '(\\w+)' in the calculator")]
        public void Given_I_Enter_RomanNumber_In_Calculator(string romanNumber)
        {
            _calculator.SetRomanNumber(romanNumber);
        }

        [When("I convert the roman numeral")]
        public void When_I_Convert_Roman_Numeral()
        {
            _calculator.ConvertToInt();
        }

        [Then("the displayed value is '(\\d+)'")]
        public async Task Then_Displayed_Value_Is_Number(int value)
        {
            Assert.AreEqual(value,await _calculator.GetIntValueAsync());
        }
    }
}
