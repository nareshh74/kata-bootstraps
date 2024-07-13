using System;

namespace BankingKata
{
    public record Transaction
    {
        public TransactionType Type { get; }
        public int Amount { get; }
        public DateOnly Date { get; }
        public int Balance { get; }

        public Transaction(TransactionType type, int amount, DateOnly date, int balance)
        {
            this.Type = type;
            this.Amount = amount;
            this.Date = date;
            this.Balance = balance;
        }
    }

    public enum TransactionType
    {
        Credit = 1,
        Debit = 0
    }
}