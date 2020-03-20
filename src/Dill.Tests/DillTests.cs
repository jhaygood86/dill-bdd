using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dill.Tests.FeatureContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dill.Tests
{
    [TestClass]
    public class DillTests
    {
        private DillTestFeatureContext _context;

        [TestInitialize]
        public void OnInitialize()
        {
            Dill dill = new Dill
            {
                FeatureLoaderType = FeatureLoaderType.EmbeddedResource,
                FeatureBasePath = "Dill.Tests"
            };

            _context = dill.GetFeatureContext<DillTestFeatureContext>();
        }

        [TestMethod]
        public async Task Test_Optional_Parameter_Null()
        {
            await _context.ExecuteScenarioAsync("Test optional parameters when null");
        }

        [TestMethod]
        public async Task Test_Optional_Parameter_NotNull()
        {
            await _context.ExecuteScenarioAsync("Test optional parameters when supplied");
        }
    }
}
