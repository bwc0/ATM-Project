using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromptNameSpace;
using System.IO;
using AccountNamespace;

namespace BankAccountApplication.UnitTests
{
    [TestClass]
    public class BankTest
    {
        Bank bank;
        User user;
        Account account;

        [TestInitialize]
        public void SetUp()
        {
            bank = new Bank();
            user = new User("username", "password", "User", "Name");
            account = new Checking(1200, 1234, true);
        }

        [TestMethod]
        public void SelectUser_UserExist_ReturnUser()
        {
            bank.Users.Add(user);
            var temp = new User("username", "password", "User", "Name");

            var result = bank.SelectUser(bank.Users, temp);

            Assert.AreEqual(user.Username, result.Username);
        }

        [TestMethod]
        public void SelectUser_UserDoesNotExist_ThrowException()
        {
            var expected = "Username or password is incorrect.";
            var temp = new User("temp", "anotherPassword", "AnotherUser", "AnotherName");


            Exception result = Assert.ThrowsException<Exception>
                (() => bank.SelectUser(bank.Users, temp));

            Assert.AreEqual(expected, result.Message);
        }

        [TestMethod]
        public void SelectAccount_AccountExist_ReturnAccount()
        {
            user.Accounts.Add(account);
            var temp = new Checking(1200, 1234, true);

            var result = bank.SelectAccount(user, temp);

            Assert.AreEqual(account.Pin, result.Pin);
        }

        [TestMethod]
        public void SelectAccount_AccountDoesNotExist_ThrowException()
        {
            var expected = "Account pin is incorrect.";
            var temp = new Checking(1200, 1234, true);


            Exception result = Assert.ThrowsException<Exception>
                (() => bank.SelectAccount(user, temp));

            Assert.AreEqual(expected, result.Message);
        }
    }

}
