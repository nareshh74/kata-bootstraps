using System.Collections.Generic;

namespace BankingKata
{
    public class Account
    {
        private readonly List<Transaction> _transactions;

        public Account()
        {
            this._transactions = new List<Transaction>();
        }

        public void Deposit(int amount)
        {
            this._transactions.Add(new Transaction(TransactionType.Credit, amount));
        }

        public IReadOnlyList<Transaction> GetTransactions()
        {
            return this._transactions.AsReadOnly();
        }

        public void Withdraw(int amount)
        {
            this._transactions.Add(new Transaction(TransactionType.Debit, amount));
        }
    }
}