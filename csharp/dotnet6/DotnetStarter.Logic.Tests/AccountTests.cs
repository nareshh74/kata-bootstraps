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
    }
}