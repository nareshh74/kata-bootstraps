using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private static readonly MoneyCollection DefaultMoneyStock = new(
            new Dictionary<Money, int>
            {
                { Money.FiveHundred, 2 },
                { Money.TwoHundred, 3 },
                { Money.Hundred, 5 },
                { Money.Fifty, 12 },
                { Money.Twenty, 20 },
                { Money.Ten, 50 },
                { Money.Five, 100 },
                { Money.Two, 250 },
                { Money.One, 500 }
            }
            );
        private static readonly Display DefaultDisplay = new();

        public static IAtm Create(MoneyCollection moneyCollection = null, Display display = null)
        {
            moneyCollection ??= Atm.DefaultMoneyStock;
            display ??= Atm.DefaultDisplay;
            return new AtmV2(new Atm(moneyCollection, display));
        }

        private Atm(MoneyCollection moneyCollection, Display display)
        {
            this._moneyCollection = moneyCollection;
            this._display = display;
        }

        private MoneyCollection DetectMoney(int amount)
        {
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
            this._display.Print(moneyCollection.ToString());
        }

        private class AtmV2 : IAtm
        {
            private readonly Atm _atm;

            public AtmV2(Atm atm)
            {
                this._atm = atm;
            }

            public void WithDraw(int amount)
            {
                if (this._atm._moneyCollection.Value < amount)
                {
                    this._atm._display.Print("Not enough money in ATM.");
                    return;
                }

                this._atm.WithDraw(amount);
            }
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
                return this._moneyCountMap.Select(x => x.Value * x.Key.Value).Sum();
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var money in this.MoneyCountMap.Keys)
            {
                var count = this.MoneyCountMap[money];
                var moneyType = count > 1 ? money.Type + "s" : money.Type.ToString();
                stringBuilder.AppendLine($"{count} {moneyType.ToLower()} of {money.Value}.");
            }
            return stringBuilder.ToString();
        }
    }

    public record Money
    {
        public static Money FiveHundred => new(500, MoneyType.Bill);
        public static Money TwoHundred => new(200, MoneyType.Bill);
        public static Money Hundred => new(100, MoneyType.Bill);
        public static Money Fifty => new(50, MoneyType.Bill);
        public static Money Twenty => new(20, MoneyType.Bill);
        public static Money Ten => new(10, MoneyType.Bill);
        public static Money Five => new(5, MoneyType.Bill);
        public static Money Two => new(2, MoneyType.Coin);
        public static Money One => new(1, MoneyType.Coin);

        public int Value { get; }
        public MoneyType Type { get; }

        private Money(int value, MoneyType moneyType)
        {
            this.Value = value;
            this.Type = moneyType;
        }
    }

    public enum MoneyType
    {
        Bill,
        Coin
    }

    public class Display
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
