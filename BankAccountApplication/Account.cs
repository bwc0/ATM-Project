using System;
using BankAccountApplication;
using PromptNameSpace;

namespace AccountNamespace
{
    public class Account
    {
        public Account(int pin)
        {
            Pin = pin;
        }

        public Account(AccountType type, int pin)
        {
            Type = type;
            AccountNumber = Guid.NewGuid();
            AccountStatus = "open";
            Balance = 0;
            Pin = pin;
        }

        public Account(AccountType type, int pin, decimal balance, bool overDraftProtection)
        {
            Type = type;
            Pin = pin;
            AccountNumber = Guid.NewGuid();
            AccountStatus = "open";
            Balance = balance;
            OverdraftProtection = overDraftProtection;
        }

        public Account(AccountType type, int pin, decimal balance, bool overDraftProtection, 
            User user)
        {
            Type = type;
            Pin = pin;
            AccountNumber = Guid.NewGuid();
            AccountStatus = "open";
            Balance = balance;
            GetUser = user;
            OverdraftProtection = overDraftProtection;
        }

        public AccountType Type { get; set; }
        public int Pin { get; set; }
        public Guid AccountNumber { get; set; }
        public string AccountStatus { get; set; }
        public decimal Balance { get; set; }
        public decimal InterestRate { get; set; }
        public bool OverdraftProtection { get; set; }
        public User GetUser { get; set; }
        IUserInput prompt = new Prompt();

        public decimal Debit(decimal amount)
        {
            CheckIfClosed(this, AccountStatus);

            if (Balance < amount)
            {
                if (OverdraftProtection == true)
                {
                    Denied();
                    prompt.GiveMessage("Cancelling withdrawl. Insufficent funds..");
                    return Balance;
                }

                Balance = Balance - amount - 15;

                prompt.GiveMessage("Insuffient funds. You have been charged " +
                    "an overdraft fee of $15.00. Balance: " + Balance);
                    

                Approved();
                return Balance;
            }

            Approved();
            return Balance -= amount;
        }


        public decimal Credit(decimal amount)
        {
            CheckIfClosed(this, AccountStatus);
            Approved();
            return Balance += amount;
        }

        public bool Transfer(Account source, Account destination, 
            decimal amount)
        {
            // Checks if user owns both accounts
            if(source.GetUser != destination.GetUser)
            {
                return false;
            }

            //Check if accounts are open
            CheckIfClosed(source, source.AccountStatus);
            CheckIfClosed(destination, destination.AccountStatus);

            source.Debit(amount);
            destination.Credit(amount);

            return true;
        }

        private void CheckIfClosed(Account account, string status)
        {
            if(status == "closed")
            {
                Denied();
                throw new Exception($"Account # is closed: {account.AccountNumber}");
            }
        }

        private void Approved()
        {
            prompt.GiveMessage("\nApproved");
        }

        private void Denied()
        {
            prompt.GiveMessage("\nApproved");
        }
    }

    public enum AccountType { Checking, Savings, Investment }

    public class Checking : Account
    {
        public Checking(decimal balance, int pin, bool overDraftProtection) 
            : base(AccountType.Checking, pin, balance, overDraftProtection) { }

        public Checking(decimal balance, int pin, bool overDraftProtection, User user)
            : base(AccountType.Checking, pin, balance, overDraftProtection, user) { }
    }

    public class Savings : Account
    {
        public Savings(int pin) : base(AccountType.Savings, pin) { }

        public Savings(decimal balance, int pin, bool overDraftProtection) 
            : base(AccountType.Savings, pin, balance, overDraftProtection) { }

        public Savings(decimal balance, int pin, bool overDraftProtection, User user)
            : base(AccountType.Savings, pin, balance, overDraftProtection, user) { }
    }
}
