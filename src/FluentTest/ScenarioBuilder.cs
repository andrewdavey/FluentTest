using System.Collections.Generic;

namespace FluentTest
{
    public class ScenarioBuilder
    {
        readonly List<IScenario> scenarios = new List<IScenario>();

        public void AddScenario(IScenario scenario)
        {
            scenarios.Add(scenario);
        }

        public IEnumerable<IScenario> GetScenarios()
        {
            return scenarios;
        }
    }
}
