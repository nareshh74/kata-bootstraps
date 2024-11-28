using System;
using System.Collections.Generic;

namespace BankingKata
{
    public class Account
    {
        private readonly List<Transaction> _transactions;
        private int _currentBalance;

        public Account()
        {
            this._transactions = new List<Transaction>();
        }

        public void Deposit(int amount)
        {
            this._currentBalance += amount;
            this._transactions.Add(new Transaction(TransactionType.Credit,
                amount,
                DateOnly.FromDateTime(DateTime.UtcNow),
                this._currentBalance));
        }

        public IReadOnlyList<Transaction> GetTransactions()
        {
            return this._transactions.AsReadOnly();
        }

        public void Withdraw(int amount)
        {
            if (this._currentBalance < amount)
            {
                throw new InvalidOperationException("Insufficient funds");
            }
            this._currentBalance -= amount;
            this._transactions.Add(new Transaction(TransactionType.Debit,
                amount,
                DateOnly.FromDateTime(DateTime.UtcNow),
                this._currentBalance));
        }
    }
}