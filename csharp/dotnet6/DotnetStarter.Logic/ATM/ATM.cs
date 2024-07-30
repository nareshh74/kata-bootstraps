using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetStarter.Logic.ATM.Domain
{
    public interface IAtm
    {
        void WithDraw(int amount);
    }

    public class Atm : IAtm
    {
        private readonly MoneyCollection _moneyCollection;
        private readonly Display _display;

        private static readonly MoneyCollection DefaultMoneyStock = new(new Dictionary<Money, int> { { new Money { Value = 100, Type = MoneyType.Bill }, 10 } });
        private static readonly Display DefaultDisplay = new();

        public static Atm Create(MoneyCollection moneyCollection = null, Display display = null)
        {
            moneyCollection ??= Atm.DefaultMoneyStock;
            display ??= Atm.DefaultDisplay;
            return new Atm(moneyCollection, display);
        }

        private Atm(MoneyCollection moneyCollection, Display display)
        {
            this._moneyCollection = moneyCollection;
            this._display = display;
        }

        private MoneyCollection DetectMoney(int amount)
        {
            // convert amount to money
            var moneyCollection = new Dictionary<Money, int>();
            var moneyStock = this._moneyCollection.MoneyCountMap;
            var moneyStockKeys = moneyStock.Keys.OrderByDescending(x => x.Value).ToList();
            var remainingAmount = amount;
            foreach (var money in moneyStockKeys)
            {
                var moneyCount = remainingAmount / money.Value;
                if (moneyCount > 0)
                {
                    var availableMoneyCount = moneyStock[money];
                    var actualMoneyCount = moneyCount > availableMoneyCount ? availableMoneyCount : moneyCount;
                    moneyCollection.Add(money, actualMoneyCount);
                    remainingAmount -= actualMoneyCount * money.Value;
                }
            }
            return new MoneyCollection(moneyCollection);
        }

        public void WithDraw(int amount)
        {
            var moneyCollection = this.DetectMoney(amount);
            this._display.Print(moneyCollection);
        }
    }

    public record MoneyCollection
    {
        private readonly Dictionary<Money, int> _moneyCountMap;

        public IReadOnlyDictionary<Money, int> MoneyCountMap => this._moneyCountMap;

        public MoneyCollection(Dictionary<Money, int> moneyCollection)
        {
            this._moneyCountMap = moneyCollection;
        }

        public int Value
        {
            get
            {
                return this._moneyCountMap.Select(x => x.Value).Sum();
            }
        }
    }

    public record Money
    {
        public int Value { get; set; }
        public MoneyType Type { get; set; }

    }

    public enum MoneyType
    {
        Bill,
        Coin
    }

    public class Display
    {
        public void Print(MoneyCollection moneyCollection)
        {
            foreach (var money in moneyCollection.MoneyCountMap.Keys)
            {
                var count = moneyCollection.MoneyCountMap[money];
                var moneyType = count > 1 ? money.Type + "s" : money.Type.ToString();
                Console.WriteLine($"{count} {moneyType.ToLower()} of {money.Value}.");
            }
        }
    }
}
