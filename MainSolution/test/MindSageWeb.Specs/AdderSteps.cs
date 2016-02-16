using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MindSageWeb.Specs
{
    [Binding]
    public class AdderSteps
    {
        [Given(@"(.*) \+ (.*)")]
        public void Given(int p0, int p1)
        {
        }

        [Then(@"it should be (.*)")]
        public void ThenItShouldBe(int p0)
        {
        }
    }
}

