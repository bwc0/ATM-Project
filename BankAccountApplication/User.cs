using System.Collections.Generic;
using AccountNamespace;


namespace BankAccountApplication
{
    public class User
    {
        public User(string username, string pin, 
            string firstName, string lastName)
        {
            Username = username;
            Pin = pin;
            FirstName = firstName;
            LastName = lastName;
            Accounts = new List<Account>();
        }

        public User(string username, string pin)
        {
            Username = username;
            Pin = pin;
            Accounts = new List<Account>();
        }

        public string Username { get; set; }
        public string Pin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Account> Accounts { get; set; }

        public User AddAccount(Account account)
        {
            account.GetUser = this;
            Accounts.Add(account);
            return this;
        }

        public Account OpenAccount(int choice, decimal balance, int pin, 
            bool overdraftProtection)
        {
            Account account;

            switch (choice)
            {
                case 1:
                    account = new Checking(balance, pin, overdraftProtection);
                    break;

                case 2:
                    account = new Savings(balance, pin, overdraftProtection);
                    break;

                default:
                    account = new Checking(balance, pin, overdraftProtection);
                    break;
            }

            AddAccount(account);

            return account;
        }

        public bool CloseAccount(Account account)
        {
            if (account.Balance != 0)
            {
                return false;
            }

            account.AccountStatus = "closed";
            return true;
        }

    }
}
