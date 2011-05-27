using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;
using System.Linq.Expressions;

namespace FluentTest
{
    public class Scenario<T> : IScenario
    {
        public Scenario(Func<T> createContext)
        {
            this.createContext = createContext;
        }

        readonly Func<T> createContext;
        When<T> action;
        readonly List<IThen<T>> assertions = new List<IThen<T>>();

        public When<T> When(Action<T> action)
        {
            return this.action = new When<T>(this, action);
        }

        public Then<T> Then(Expression<Func<T, bool>> assertion)
        {
            var then = new Then<T>(this, delegate { }, assertion);
            AddAssertion(then);
            return then;
        }

        public IEnumerable<ITestCommand> CreateTestCommands(IMethodInfo method)
        {
            foreach (var assertion in assertions)
            {
                yield return assertion.CreateTestCommand(method);
            }
        }

        public void AddAssertion(IThen<T> assertion)
        {
            assertions.Add(assertion);
        }

        public T CreateContext()
        {
            return createContext();
        }
    }

}
