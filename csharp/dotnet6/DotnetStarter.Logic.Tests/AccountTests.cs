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
            Assert.Equal(1, transactions.Count());
            Assert.Equal(amount, transactions[0].Amount);
            Assert.Equal(TransactionType.Credit, transactions[0].Amount);
        }
    }
}