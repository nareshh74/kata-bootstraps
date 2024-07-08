using System;
using System.Collections.Generic;

namespace DotnetStarter.Logic.ElephantCarpaccio
{
    public class PriceCalculator
    {
        protected PriceCalculator()
        {
        }

        public static PriceCalculator Create()
        {
            return new PriceCalculatorWithDefaultTax();
        }

        public static PriceCalculator Create(string stateCode)
        {
            return new PriceCalculatorAsPerStateCode(stateCode);
        }

        public virtual decimal GetTotalPrice(int itemCount, decimal itemPrice)
        {
            return itemCount * itemPrice;
        }
    }

    public class PriceCalculatorWithDefaultTax : PriceCalculator
    {
        internal PriceCalculatorWithDefaultTax() : base()
        {
        }

        public override decimal GetTotalPrice(int itemCount, decimal itemPrice)
        {
            return base.GetTotalPrice(itemCount, itemPrice) * 1.03m;
        }
    }

    public class PriceCalculatorAsPerStateCode : PriceCalculator
    {
        private static readonly Dictionary<string, decimal> _stateTaxRates = new Dictionary<string, decimal>
        {
            { "CA", 8.25m },
            { "AL", 4m },
            { "TX", 6.25m },
            { "NV", 8m },
            { "UT", 6.85m }
        };
        private readonly string _stateCode;

        internal PriceCalculatorAsPerStateCode(string stateCode) : base()
        {
            if (!PriceCalculatorAsPerStateCode._stateTaxRates.ContainsKey(stateCode))
            {
                throw new ArgumentException("Invalid state code", nameof(stateCode));
            }
            this._stateCode = stateCode;
        }

        public override decimal GetTotalPrice(int itemCount, decimal itemPrice)
        {
            var totalPrice = base.GetTotalPrice(itemCount, itemPrice);
            var taxRate = PriceCalculatorAsPerStateCode._stateTaxRates[_stateCode];
            return Math.Round(totalPrice * (1 + taxRate / 100), 2);
        }
    }
}
