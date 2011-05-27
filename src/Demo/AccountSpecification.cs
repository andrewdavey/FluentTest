using System;
using FluentTest;

namespace Demo
{
    public class AccountSpecification : Specification
    {
        [Specification]
        public void Creation()
        {
            Given(new Account("John Smith"))
                .Then(the => the.Balance == 0)
                .And(the => the.Name == "John Smith");

        }

        [Specification]
        public void Deposit()
        {
            var emptyAccount = Context(() => new Account("John Smith"));

            Given(emptyAccount)
                .When(account => account.Deposit(100))
                .Then(the => the.Balance == 100);

            Given(emptyAccount)
                .When(account => account.Deposit(-1))
                .ThenException<ArgumentException>();

            Given(emptyAccount)
                .When(account => account.Deposit(0))
                .ThenException<ArgumentException>(
                    ex => ex.ParamName == "amount"
                );//.And(ex => ...)
        }

        [Specification]
        public void Transfer()
        {
            var twoAccounts = Context(delegate
            {
                var from = new Account("A");
                var to = new Account("B");
                from.Deposit(100);
                return new { from, to };
            });

            Given(twoAccounts)
            .When(c => c.from.Transfer(25, c.to))
            .Then(c => c.from.Balance == 75)
             .And(c => c.to.Balance == 25);
        }

    }

}
