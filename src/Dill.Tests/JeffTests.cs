using System.Threading.Tasks;
using Dill.Tests.FeatureContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dill.Tests
{
    [TestClass]
    public class JeffTests
    {
        private JeffFeatureContext _context;

        [TestInitialize]
        public void OnInitialize()
        {
            Dill dill = new()
            {
                FeatureLoaderType = FeatureLoaderType.EmbeddedResource,
                FeatureBasePath = "Dill.Tests"
            };

            _context = dill.GetFeatureContext<JeffFeatureContext>();
        }

        [TestMethod]
        public async Task Test_JeffReturnsFaultyMicrowave()
        {
            await _context.ExecuteScenarioAsync("Jeff returns a faulty microwave");
        }

        [TestMethod]
        public async Task Test_JeffReturnsFaultyMicrowave_WithoutReceipt()
        {
            await _context.ExecuteScenarioAsync("Jeff returns a faulty microwave without receipt");
        }
    }
}
