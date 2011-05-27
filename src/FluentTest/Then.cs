using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Xunit.Sdk;

namespace FluentTest
{
    public class Then<T> : IThen<T>
    {
        public Then(Scenario<T> given, Action<T> action, Expression<Func<T, bool>> assertion)
        {
            this.given = given;
            this.action = action;
            this.assertion = assertion;

            displayName = assertion.Body.ToString();

            var frame = new StackTrace(true).GetFrames().Skip(2).Take(1).FirstOrDefault();
            if (frame != null)
            {
                location = string.Format("{0}({1},{2}): at {3}.{4}()", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber(), frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name) + Environment.NewLine + ".";
            }
        }

        readonly Scenario<T> given;
        readonly Action<T> action;
        readonly Expression<Func<T, bool>> assertion;
        readonly string displayName;
        readonly string location;

        public Then<T> And(Expression<Func<T, bool>> assertion)
        {
            var then = new Then<T>(given, action, assertion);
            given.AddAssertion(then);
            return then;
        }

        MethodResult Execute(IMethodInfo method)
        {
            try
            {
                var context = given.CreateContext();
                action(context);

                var predicate = assertion.Compile();
                if (predicate(context))
                {
                    return new PassedResult(method, displayName);
                }
                else
                {
                    return new FailedResult(
                        method.Name, method.TypeName, displayName,
                        null,
                        "", "", location
                    );
                }

            }
            catch (Exception ex)
            {
                return new FailedResult(method, ex, displayName);
            }
        }

        public ITestCommand CreateTestCommand(IMethodInfo method)
        {
            return new ScenarioAssertionTestCommand(displayName, () =>
            {
                return Execute(method);
            });
        }
    }

}
