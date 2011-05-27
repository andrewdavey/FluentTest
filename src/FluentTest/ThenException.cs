using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Xunit.Sdk;

namespace FluentTest
{

    public class ThenException<T, TException> : IThen<T>
        where TException : Exception
    {
        public ThenException(Scenario<T> given, Action<T> action)
        {
            this.given = given;
            this.action = action;

            displayName = "Expect " + typeof(T).FullName;

            var frame = new StackTrace(true).GetFrames().Skip(2).Take(1).FirstOrDefault();
            if (frame != null)
            {
                location = string.Format("{0}({1},{2}): at {3}.{4}()", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber(), frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name) + Environment.NewLine + ".";
            }
        }

        public ThenException(Scenario<T> given, Action<T> action, Expression<Func<TException, bool>> assertion)
        {
            this.assertion = assertion;
            this.given = given;
            this.action = action;

            displayName = "Expect " + typeof(T).FullName;

            var frame = new StackTrace(true).GetFrames().Skip(2).Take(1).FirstOrDefault();
            if (frame != null)
            {
                location = string.Format("{0}({1},{2}): at {3}.{4}()", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber(), frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name) + Environment.NewLine + ".";
            }
        }

        readonly Scenario<T> given;
        readonly Action<T> action;
        readonly string location;
        readonly string displayName;
        readonly Expression<Func<TException, bool>> assertion;

        MethodResult Execute(IMethodInfo method)
        {
            try
            {
                var context = given.CreateContext();
                action(context);
                return new FailedResult(
                    method.Name, method.TypeName, displayName,
                    null,
                    "",
                    typeof(T).FullName + " was not thrown",
                    location
                );
            }
            catch (Exception ex)
            {
                if (ex is TException)
                {
                    if (assertion != null)
                    {
                        var predicate = assertion.Compile();
                        if (predicate((TException)ex) == false)
                        {
                            return new FailedResult(
                                method.Name, method.TypeName, displayName, null,
                                "", "Assertion failed: " + assertion.Body.ToString(),
                                location
                            );
                        }
                    }
                    return new PassedResult(method, displayName);
                }
                else
                {
                    return new FailedResult(
                        method.Name, method.TypeName, displayName, null,
                        "",
                        string.Format("{0} was thrown, but expected {1}", ex.GetType().FullName, typeof(TException).FullName),
                        location
                    );
                }
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
