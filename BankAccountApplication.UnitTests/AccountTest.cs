using System;
using AccountNamespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankAccountApplication.UnitTests
{
    [TestClass]
    public class AccountTest
    {
        private Account account;
        private Account anotherAccount;
        private User user;
        private Guid accountNumber = Guid.NewGuid();
        private decimal balance = 1000;
        private int pin = 4321;
        private readonly bool overDraftProtection = true;


        [TestInitialize]
        public void SetUp()
        {
            user = new User("username", "password", "User", "Name");

            account = new Checking(balance, pin, overDraftProtection, user)
            {
                AccountNumber = accountNumber
            };

            anotherAccount = new Savings(1000, 5432, true, user);

            user.AddAccount(account);
        }

        [TestMethod]
        public void Debit_SuffientFundsAndOpen_ReturnBalance()
        {
            var expected = 250;
            var amount = 750;

            var result = account.Debit(amount);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Debit_AccountClosed_ThrowException()
        {
            var expected = $"Account # is closed: {account.AccountNumber}";
            account.AccountStatus = "closed";

            var result = Assert.ThrowsException<Exception>
                (() => account.Debit(600));

            Assert.AreEqual(expected, result.Message);
        }

        [TestMethod]
        public void 
            Debit_InsuffientFundsAndOverDraftProtectionEnabled_ReturnBalance()
        {
            var expected = 1000;
            var amount = 1100;

            var result = account.Debit(amount);

            // Balance should not change;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void
            Debit_InsuffientFundsAndOverDraftProtectionDisabled_ReturnBalance()
        {
            var expected = -215;
            var amount = 1200;
            account.OverdraftProtection = false;

            var result = account.Debit(amount);

            // Balance should Debit amount and charge fee
            Assert.AreEqual(expected, account.Balance);
        }

        [TestMethod]
        public void Credit_AccountIsOpen_ReturnBalance()
        {
            var expected = 1500;
            var amount = 500;

            var result = account.Credit(amount);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Credit_AccountIsClosed_ThrowsException()
        {
            var expected = $"Account # is closed: {account.AccountNumber}";
            account.AccountStatus = "closed";

            var result = Assert.ThrowsException<Exception>
                (() => account.Credit(500));

            Assert.AreEqual(expected, result.Message);
        }

        [TestMethod]
        public void Transfer_SourceAccountSufficientFunds_ReturnBalance()
        {
            var amount = 500;
            user.AddAccount(anotherAccount);

            var result = account.Transfer(account, anotherAccount, amount);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transfer_SourceAccountClosed_ThrowException()
        {
            var expected = $"Account # is closed: {account.AccountNumber}";
            account.AccountStatus = "closed";

            var amount = 600;

            var result = Assert.ThrowsException<Exception>(
                () => account.Transfer(account, anotherAccount, amount));

            Assert.AreEqual(expected, result.Message);
        }

        [TestMethod]
        public void Transfer_DestinationAccountClosed_ThrowException()
        {
            var expected = $"Account # is closed: {anotherAccount.AccountNumber}";
            anotherAccount.AccountStatus = "closed";

            var amount = 600;

            var result = Assert.ThrowsException<Exception>(
                () => account.Transfer(account, anotherAccount, amount));

            Assert.AreEqual(expected, result.Message);
        }

        [TestMethod]
        public void Transer_SourceUserDoesNotEqualDestinationUser_ReturnFalse()
        {
            Account testAccount = new Account(AccountType.Checking, 87654)
            {
                GetUser = new User("testUsername", "password", "Test", "User")
            };

            var result = account.Transfer(account, testAccount, 500);

            Assert.IsFalse(result);
        }
    }
}
