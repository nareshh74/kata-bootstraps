namespace BankingKata
{
    public record Transaction
    {
        public TransactionType Type { get; }
        public int Amount { get; }

        public Transaction(TransactionType type, int amount)
        {
            this.Type = type;
            this.Amount = amount;
        }
    }

    public enum TransactionType
    {
        Credit = 1,
        Debit = 0
    }
}