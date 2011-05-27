using System;

namespace Demo
{
    public class Account
    {
        public Account(string name)
        {
            Name = name;
        }
        
        public int Balance { get; private set; }
        public string Name { get; set; }

        public void Deposit(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.", "amount");
            Balance += amount;
        }

        public void Transfer(int amount, Account account)
        {
            Balance -= amount;
            account.Deposit(amount);
        }
    }
}
