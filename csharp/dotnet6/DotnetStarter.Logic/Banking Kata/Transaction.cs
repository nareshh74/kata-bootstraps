namespace BankingKata
{
    public record Transaction
    {
        public TransactionType Type { get; init; }
        public int Amount { get; init; }
    }

    public enum TransactionType
    {
        Credit = 1,
        Debit = 0
    }
}