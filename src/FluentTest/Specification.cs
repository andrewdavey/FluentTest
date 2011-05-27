using System;
using System.Collections.Generic;

namespace FluentTest
{
    public class Specification
    {
        public Func<T> Context<T>(Func<T> create)
        {
            return create;
        }

        public Scenario<T> Given<T>(T context)
        {
            return Given(() => context);
        }

        public Scenario<T> Given<T>(Func<T> createContext)
        {
            var scenario = new Scenario<T>(createContext);
            ScenarioBuilder.AddScenario(scenario);
            return scenario;
        }

        public ScenarioBuilder ScenarioBuilder { get; set; }
    }
}
