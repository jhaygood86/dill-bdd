using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dill.Tests.FeatureContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dill.Tests
{
    [TestClass]
    public class RomanNumeralTests
    {
        private RomanNumeralFeatureContext _context;

        [TestInitialize]
        public void OnInitialize()
        {
            Dill dill = new Dill
            {
                FeatureLoaderType = FeatureLoaderType.EmbeddedResource,
                FeatureBasePath = "Dill.Tests"
            };

            _context = dill.GetFeatureContext<RomanNumeralFeatureContext>();
        }

        [TestMethod]
        public async Task Test_RomanNumeral_CalculatorShouldTransformRomanNumeral()
        {
            await _context.ExecuteScenarioAsync("The calculator should transform simple roman numeral to number");
        }
    }
}
