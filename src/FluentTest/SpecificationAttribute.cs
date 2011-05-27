using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentTest
{
    public class SpecificationAttribute : FactAttribute
    {
        protected override IEnumerable<ITestCommand> EnumerateTestCommands(IMethodInfo method)
        {
            var specification = (Specification)method.CreateInstance();
            var builder = new ScenarioBuilder();
            specification.ScenarioBuilder = builder;
            method.Invoke(specification);
            foreach (var scenario in builder.GetScenarios())
            {
                foreach (var command in scenario.CreateTestCommands(method))
                {
                    yield return command;
                }
            }
        }
    }

}
