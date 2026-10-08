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
        private readonly IMoneyDetectionBehavior _moneyDetectionBehavior;

        public static readonly MoneyCollection DefaultMoneyStock = new(
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
        public static readonly Display DefaultDisplay = new();

        public static IAtm Create(MoneyCollection moneyCollection, Display display)
        {
            IMoneyDetectionBehavior moneyDetectionBehavior = new MoneyDetectionBehaviorV2();
            return new AtmV2(new Atm(moneyCollection, display, moneyDetectionBehavior));
        }

        private Atm(
            MoneyCollection moneyCollection,
            Display display,
            IMoneyDetectionBehavior moneyDetectionBehavior)
        {
            // don't directly assign stateful properties to parameters passed by reference
            this._moneyCollection = new MoneyCollection(moneyCollection.MoneyCountMap);
            this._display = display;
            this._moneyDetectionBehavior = moneyDetectionBehavior;
        }

        private MoneyCollection DetectMoney(int amount)
        {
            return this._moneyDetectionBehavior.DetectMoney(amount, this._moneyCollection);
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

        private interface IMoneyDetectionBehavior
        {
            MoneyCollection DetectMoney(int amount, MoneyCollection moneyCollection);
        }

        /// <summary>
        /// initial impl.
        /// Doesn't actually detect money from the ATM. - BUG!!
        /// </summary>
        private class MoneyDetectionBehaviorV1 : IMoneyDetectionBehavior
        {
            public MoneyCollection DetectMoney(int amount, MoneyCollection moneyCollection)
            {
                var moneyToBeWithdrawn = new Dictionary<Money, int>();
                var moneyInAtm = moneyCollection.MoneyCountMap;
                var denominations = moneyInAtm.Keys.OrderByDescending(x => x.Value).ToList();
                var remainingAmount = amount;
                foreach (var denomination in denominations)
                {
                    var neededDenominationCount = remainingAmount / denomination.Value;
                    if (neededDenominationCount > 0)
                    {
                        var availableDenominationCount = moneyInAtm[denomination];
                        var denominationCountToBeDetected = neededDenominationCount > availableDenominationCount ? availableDenominationCount : neededDenominationCount;
                        moneyToBeWithdrawn.Add(denomination, denominationCountToBeDetected);
                        remainingAmount -= denominationCountToBeDetected * denomination.Value;
                    }
                }
                return new MoneyCollection(moneyToBeWithdrawn);
            }
        }

        private class MoneyDetectionBehaviorV2 : IMoneyDetectionBehavior
        {
            public MoneyCollection DetectMoney(int amount, MoneyCollection moneyCollection)
            {
                var moneyToBeWithdrawn = new Dictionary<Money, int>();
                var moneyInAtm = moneyCollection.MoneyCountMap;
                var denominations = moneyInAtm.Keys.OrderByDescending(x => x.Value).ToList();
                var remainingAmount = amount;
                foreach (var denomination in denominations)
                {
                    var availableDenominationCount = moneyInAtm[denomination];
                    if (availableDenominationCount == 0)
                    {
                        continue;
                    }
                    var neededDenominationCount = remainingAmount / denomination.Value;
                    if (neededDenominationCount > 0)
                    {
                        var denominationCountToBeDetected = neededDenominationCount > availableDenominationCount ? availableDenominationCount : neededDenominationCount;
                        if (moneyCollection.TryDetectMoney(denomination, denominationCountToBeDetected))
                        {
                            moneyToBeWithdrawn.Add(denomination, denominationCountToBeDetected);
                            remainingAmount -= denominationCountToBeDetected * denomination.Value;
                        }
                    }
                }
                return new MoneyCollection(moneyToBeWithdrawn);
            }
        }
    }

    public record MoneyCollection
    {
        private readonly Dictionary<Money, int> _moneyCountMap;

        public IReadOnlyDictionary<Money, int> MoneyCountMap => this._moneyCountMap;

        public MoneyCollection(IEnumerable<KeyValuePair<Money, int>> moneyCollection)
        {
            // don't directly assign parameters passed by reference in a Value Object
            var moneyCollectionCopy = moneyCollection.ToDictionary(money => money.Key, money => money.Value);
            this._moneyCountMap = moneyCollectionCopy;
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

        public bool TryDetectMoney(Money money, int count)
        {
            if (this._moneyCountMap[money] < count)
            {
                return false;
            }
            this._moneyCountMap[money] -= count;
            return true;
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
