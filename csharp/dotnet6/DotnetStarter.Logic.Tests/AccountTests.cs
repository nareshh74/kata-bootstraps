using BankingKata;
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

            // Act
            account.Withdraw(amount);

            // Assert
            var transactions = account.GetTransactions();
            Assert.Single(transactions);
            Assert.Equal(amount, transactions[0].Amount);
            Assert.Equal(TransactionType.Debit, transactions[0].Type);
        }
    }
}