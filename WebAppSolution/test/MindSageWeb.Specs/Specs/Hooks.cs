using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MindSageWeb.Specs
{
    [Binding]
    public sealed class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
        }

        [BeforeScenarioBlock("mock")]
        public void BeforeScenarioBlock()
        {
            if (ScenarioContext.Current.CurrentScenarioBlock == ScenarioBlock.Given)
            {
                var mocks = new Moq.MockRepository(Moq.MockBehavior.Loose);
                ScenarioContext.Current.Set(mocks);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
        }
    }
}
