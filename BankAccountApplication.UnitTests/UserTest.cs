using System;
using AccountNamespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankAccountApplication.UnitTests
{

    [TestClass]
    public class UserTest

    {
        private User user;
        private Account account;
        private readonly string username = "Username";
        private readonly string password = "password";
        private readonly string firstName = "User";
        private readonly string lastName = "Name";
        private readonly decimal balance = 1000;
        private readonly int accountPin = 52353;
        private readonly bool overDraftProtection = true;

        [TestInitialize]
        public void SetUp()
        {
            user = new User(username, password, firstName, lastName);
            account = new Account(AccountType.Checking, accountPin, balance, true);
        }

        [TestMethod]
        public void AddAccount_UserShouldHaveOneAccount()
        {
            var expected = 1;

            var result = user.AddAccount(account);

            Assert.AreEqual(expected, result.Accounts.Count);
        }

        [TestMethod]
        public void AddAccount_AccountShouldHaveUser()
        {
            var expected = user.Username;

            var result = user.AddAccount(account);

            Assert.AreEqual(expected, account.GetUser.Username);
        }

        [TestMethod]
        public void OpenAccount_ReturnAccount()
        {
            user.AddAccount(account);

            var result = user.OpenAccount(1, balance, 543543,
                overDraftProtection);

            Assert.IsNotNull(result);
            Assert.AreEqual(account.Type, result.Type);
            Assert.AreEqual(account.GetUser, result.GetUser);
            Assert.AreEqual(account.GetUser.Accounts.Count,
                result.GetUser.Accounts.Count);
        }

        [TestMethod]
        public void CloseAccount_BalanceIsZero_ReturnTrue()
        {
            account.Balance = 0;

            user.AddAccount(account);

            var result = user.CloseAccount(account);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CloseAccount_BalanceIsNot_ReturnFalse()
        {
            user.AddAccount(account);

            var result = user.CloseAccount(account);

            Assert.IsFalse(result);
        }

    }

}
