using Xunit.Sdk;

namespace FluentTest
{
    public interface IThen<T>
    {
        ITestCommand CreateTestCommand(IMethodInfo method);
    }
}
