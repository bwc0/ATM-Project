using System;
using AccountNamespace;
using PromptNameSpace;

namespace BankAccountApplication
{
    public class ATM
    {
        static Bank bank = new Bank();
        static IUserInput prompt = new Prompt();
        static User user;
        static Account account;

        public void Engine()
        {
            int choice = 0;
            bool atm = true;

            while(atm)
            {
                try
                {
                    choice = prompt.AskForInt("1. Sign Up" +
                    "\n2. Sign In" +
                    "\nEnter: ");
                }
                catch (FormatException fe)
                {
                    prompt.GiveMessage(fe.Message);
                }

                while (choice != 0 && atm)
                {
                    switch (choice)
                    {
                        case 1:
                            SignUp();
                            break;
                        case 2:
                            SignIn();
                            break;
                    }

                    while (choice != -1 && bank.Users.Count != 0)
                    {
                        prompt.GiveMessage("What would you like to do next?\n");

                        try
                        {
                            choice = prompt.AskForInt("1. Create Account" +
                            "\n2. Login to an Account" +
                            "\n3. Check Balance" +
                            "\n4. Withdrawl" +
                            "\n5. Deposit" +
                            "\n6. Transfer" +
                            "\n7. List Accounts" +
                            "\n8. Closed Account" +
                            "\n9. User Profile" +
                            "\n86. Signout" +
                            "\nEnter: ");
                        }
                        catch (FormatException fe)
                        {
                            prompt.GiveMessage(fe.Message);
                        }

                        switch (choice)
                        {
                            case 1:
                                NewAccount();
                                break;

                            case 2:
                                Login();
                                break;

                            case 3:
                                try
                                {
                                    prompt.GiveMessage("\nAccount Number: " +
                                    account.AccountNumber
                                    + "\nBalance: " + account.Balance);
                                }
                                catch (NullReferenceException nfe)
                                {
                                    prompt.GiveMessage("Create account or " +
                                        "Log into an account");
                                }
                                catch (Exception ex)
                                {
                                    prompt.GiveMessage(ex.Message);
                                }

                                break;

                            case 4:
                                try
                                {
                                    bank.Withdrawl(account);
                                }
                                catch (FormatException fe)
                                {
                                    prompt.GiveMessage(fe.Message);
                                }
                                catch (NullReferenceException nfe)
                                {
                                    prompt.GiveMessage("Create account or " +
                                        "Log into an account");
                                }
                                catch (Exception ex)
                                {
                                    prompt.GiveMessage(ex.Message);
                                }

                                break;

                            case 5:
                                try
                                {
                                    bank.Deposit(account);
                                }
                                catch (FormatException fe)
                                {
                                    prompt.GiveMessage(fe.Message);
                                }
                                catch (NullReferenceException nfe)
                                {
                                    prompt.GiveMessage("Create account or " +
                                        "Log into an account");
                                }
                                catch (Exception ex)
                                {
                                    prompt.GiveMessage(ex.Message);
                                }
                                break;

                            case 6:
                                Transfer();
                                break;

                            case 7:
                                ListAccounts();
                                break;

                            case 8:
                                CloseAccount();
                                break;

                            case 9:
                                prompt.GiveMessage($"\nName: {user.FirstName} {user.LastName}" +
                                    $"\nUsername: {user.Username}" +
                                    $"\nNo. of Accounts: {user.Accounts.Count}" +
                                    $"\n");

                                break;
                            case 86:
                                choice = -1;
                                break;

                            default:
                                choice = -1;
                                break;
                        }
                    }

                    choice = prompt.AskForInt("\n1. New User" +
                        "\n2. Existing User"
                        );

                }
            }
        }

        private static void SignUp()
        {
            var username = prompt.AskForString("Enter a username: ");
            var password = prompt.AskForString("Enter a password: ");
            var firstName = prompt.AskForString("Enter your first name: ");
            var lastName = prompt.AskForString("Enter your last name: ");

            user = bank.CreateUser(username, password, firstName, lastName);

            prompt.GiveMessage($"\nWelcome, {user.FirstName}.");
        }

        private static void SignIn()
        {
            var username = prompt.AskForString("Enter username: ");
            var password = prompt.AskForString("Enter password: ");

            try
            {
                user =
                bank.SelectUser(bank.Users,
                    new User(username, password));

                prompt.GiveMessage($"\nWelcome back, {user.FirstName}.");
            }
            catch (Exception ex)
            {
                prompt.GiveMessage(ex.Message);
            }
        }

        private static void NewAccount()
        {
            try
            {
                var accountType = prompt.AskForInt("Choose a number: 1. Checking " +
                                 "2. Savings. 3. Investment");
                var pin = prompt.AskForInt("Enter pin: ");
                var balance = prompt.AskForDecimal("Enter your starting balance: ");

                var decision = prompt.AskForString("Would you like " +
                "overdraft protect: y/n");

                account = bank.CreateAccount(user, accountType,
                    pin, balance, decision);

                prompt.GiveMessage("\nAccount created.");
            }
            catch(FormatException fe)
            {
                prompt.GiveMessage(fe.Message);
            }
        }

        private static void Login()
        {
            try
            {
                var pin = prompt.AskForInt("Enter pin: ");

                account = bank.SelectAccount(user,
                    bank.SignInAccount(pin));

                prompt.GiveMessage("\nCurrent account: ");
                prompt.GiveMessage($"Account Type: { account.Type }");
                prompt.GiveMessage($"Account Number: { account.AccountNumber }");

            } 
            catch (FormatException fe)
            {
                prompt.GiveMessage(fe.Message);
            }
            catch (NullReferenceException nfe)
            {
                prompt.GiveMessage("Create account or " +
                	"Log into an account");
            }
            catch (Exception ex)
            {
                prompt.GiveMessage(ex.Message);
            }

           
        }

        private static void Transfer()
        {
            prompt.GiveMessage("What account would you like to transfer too?");

            try
            {
                var pin = prompt.AskForInt("Enter pin: ");

                Account destinationAccount =
                    bank.SignInAccount(pin);

                bank.Transfer(account,
                    bank.SelectAccount(user, destinationAccount));
            }
            catch (FormatException fe)
            {
                prompt.GiveMessage(fe.Message);
            }
            catch (Exception ex)
            {
                prompt.GiveMessage(ex.Message);
            }
        }

        private static void ListAccounts()
        {
            foreach (Account temporary in user.Accounts)
            {
                prompt.GiveMessage($"\nAccount Type: {temporary.Type}" +
                    $"\nAccount Number: {temporary.AccountNumber}" +
                    $"\nAccount Status: {temporary.AccountStatus}" +
                    $"\nBalance: {temporary.Balance}" +
                    $"\nOverdraft Protection: {temporary.OverdraftProtection}" +
                    "\n");
            }
        }

        private static void CloseAccount()
        {
            try
            {
                Account accountToBeClosed =
                                bank.SelectAccount(user, account);

                bank.CloseAccount(user, accountToBeClosed);
            }
            catch (Exception ex)
            {
                prompt.GiveMessage(ex.Message);
            }
        }
    }
}
