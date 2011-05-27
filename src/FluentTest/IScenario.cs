using System.Collections.Generic;
using Xunit.Sdk;

namespace FluentTest
{
    public interface IScenario
    {
        IEnumerable<ITestCommand> CreateTestCommands(IMethodInfo method);
    }
}
