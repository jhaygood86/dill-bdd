using System;
using System.Collections.Generic;
using System.Text;
using Dill.Attributes;

namespace Dill.Tests.FeatureContext
{
    public class DillTestFeatureContext : global::Dill.FeatureContext
    {
        public DillTestFeatureContext(Dill dill) : base(dill, "dill_test.feature")
        {
        }

        [Given("foo")]
        public void Given_Foo()
        {

        }

        [When("foo does bar( twice)?")]
        public void When_Foo_Does_Bar(string twice)
        {

        }

        [Then("bar is foo")]
        public void Then_Bar_Is_Foo()
        {

        }
    }
}
