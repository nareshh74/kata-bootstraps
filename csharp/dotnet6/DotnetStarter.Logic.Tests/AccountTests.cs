using BankingKata;
using System;
using System.Linq;
using Xunit;

namespace BankingKataTests
{
    public class AccountTests
    {
        [Fact]
        public void Deposit_ShouldRecordExpectedTransaction()
        {
            // Arrange
            var account = new Account();
            var amount = 100;

            // Act
            account.Deposit(amount);

            // Assert
            var transactions = account.GetTransactions();
            Assert.Single(transactions);
            Assert.Equal(amount, transactions[0].Amount);
            Assert.Equal(TransactionType.Credit, transactions[0].Type);
        }

        [Fact]
        public void Withdraw_ShouldRecordExpectedTransaction()
        {
            // Arrange
            var account = new Account();
            var amount = 100;
            account.Deposit(amount);

            // Act
            account.Withdraw(amount);

            // Assert
            var transactions = account.GetTransactions();
            var debitTransactions = transactions.Where(txn => txn.Type == TransactionType.Debit);
            Assert.Single(debitTransactions);
            Assert.Equal(amount, debitTransactions.First().Amount);
            Assert.Equal(TransactionType.Debit, debitTransactions.First().Type);
        }

        [Fact]
        public void Withdraw_ShouldFailIfInsufficientFunds()
        {
            // Arrange
            var account = new Account();
            var amount = 100;

            // Act
            account.Deposit(amount);

            // Assert
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(amount + 1));
        }

        [Fact]
        public void GetTransactions_ShouldReturn0Transactions()
        {
            // Arrange
            var account = new Account();

            // Act
            var transactions = account.GetTransactions();

            // Assert
            Assert.Empty(transactions);
        }

        [Fact]
        public void GetTransactions_ShouldReturn1Transaction()
        {
            // Arrange
            var account = new Account();
            account.Deposit(100);

            // Act
            var transactions = account.GetTransactions();

            // Assert
            Assert.Single(transactions);
        }

        [Fact]
        public void GetTransactions_ShouldReturn2Transactions()
        {
            // Arrange
            var account = new Account();
            account.Deposit(100);
            account.Withdraw(50);

            // Act
            var transactions = account.GetTransactions();

            // Assert
            Assert.Equal(2, transactions.Count);
        }

        [Fact]
        public void GetTransactions_ShouldReturnTransactionsInOrder()
        {
            // Arrange
            var account = new Account();
            account.Deposit(100);
            account.Withdraw(50);

            // Act
            var transactions = account.GetTransactions();

            // Assert
            Assert.Equal(TransactionType.Credit, transactions[0].Type);
            Assert.Equal(TransactionType.Debit, transactions[1].Type);
        }

        [Fact]
        public void GetTransactions_ShouldReturnTransactionsWithCorrectAmounts()
        {
            // Arrange
            var account = new Account();
            account.Deposit(100);
            account.Withdraw(50);

            // Act
            var transactions = account.GetTransactions();

            // Assert
            Assert.Equal(100, transactions[0].Amount);
            Assert.Equal(50, transactions[1].Amount);
        }

        [Fact]
        public void GetTransactions_ShouldReturnTransactionsWithCorrectDate()
        {
            // Arrange
            var account = new Account();
            account.Deposit(100);
            account.Withdraw(50);

            // Act
            var transactions = account.GetTransactions();

            // Assert
            Assert.Equal(DateTime.UtcNow.Date, transactions[0].Date);
            Assert.Equal(DateTime.UtcNow.Date, transactions[1].Date);
        }
    }
}