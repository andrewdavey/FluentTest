using System;
using System.Linq.Expressions;

namespace FluentTest
{
    public class When<T>
    {
        public When(Scenario<T> given, Action<T> action)
        {
            this.given = given;
            this.action = action;
        }

        readonly Scenario<T> given;
        readonly Action<T> action;

        public Then<T> Then(Expression<Func<T, bool>> assertion)
        {
            var then = new Then<T>(given, action, assertion);
            given.AddAssertion(then);
            return then;
        }

        public ThenException<T, TException> ThenException<TException>() where TException : Exception
        {
            var then = new ThenException<T, TException>(given, action);
            given.AddAssertion(then);
            return then;
        }

        public ThenException<T, TException> ThenException<TException>(Expression<Func<TException, bool>> assertion) where TException : Exception
        {
            var then = new ThenException<T, TException>(given, action, assertion);
            given.AddAssertion(then);
            return then;
        }

        public void Execute(T context)
        {
            action(context);
        }
    }

}
