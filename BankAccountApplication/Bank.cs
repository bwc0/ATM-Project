using System;
using AccountNamespace;
using System.Collections.Generic;
using PromptNameSpace;

namespace BankAccountApplication
{
    public class Bank
    {
        public Bank()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; }
        IUserInput prompt = new Prompt();

        public void Withdrawl(Account account)
        {
            var withdrawlAmount = prompt.AskForDecimal("Enter number to withdrawl: ");
            account.Debit(withdrawlAmount);
        }

        public void Deposit(Account account)
        {
            var depositAmount = prompt.AskForDecimal("Enter number to deposit: ");
            account.Credit(depositAmount);
        }

        public void Transfer(Account sourceAccount, Account destinationAccount)
        {
            var transferAmount = prompt.AskForDecimal("Enter number to transfer: ");

            if (sourceAccount.Transfer(sourceAccount, destinationAccount,
                    transferAmount))
            {
                prompt.GiveMessage("Transfer successful.");
            }
            else
            {
                prompt.GiveMessage("Transfer failed.");
            }
        }

        public User SelectUser(List<User> users, User temp)
        {
            foreach (User user in users)
            {
                if (user.Username.Equals(temp.Username) && user.Pin == temp.Pin)
                {
                    return user;
                }
            }

            throw new Exception("Username or password is incorrect.");
        }

        public Account SelectAccount(User user, Account temp)
        {
            foreach (Account account in user.Accounts)
            {
                if(account.Pin == temp.Pin)
                {
                    return account;
                }
            }

            throw new Exception("Account pin is incorrect.");
        }
        
        public User CreateUser(string username, string pin, string firstName, 
            string lastName)
        {
            User newUser = 
                new User(username, pin, firstName, lastName);

            Users.Add(newUser);
            
            return newUser;
        }

        public Account CreateAccount(User user, int accountType, int pin, 
            decimal balance, string overDraftSelection)
        {
            var overDraftProtection = 
                false || (overDraftSelection == "y" || overDraftSelection == "yes");

            return user.OpenAccount(accountType, balance, pin,
                overDraftProtection);
        }

        public void CloseAccount(User user, Account account)
        {
            if(!user.CloseAccount(account))
            {
                prompt.GiveMessage("Account balance must be zero to close");
            }
            else
            {
                prompt.GiveMessage("Account is closed");
            }
        }

        public Account SignInAccount(int pin)
        {
            return new Account(pin);
        }
    }
}
