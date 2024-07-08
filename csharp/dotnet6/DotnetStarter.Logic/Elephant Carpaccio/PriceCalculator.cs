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
            if (stateCode != "CA")
            {
                throw new System.ArgumentException("Invalid state code", nameof(stateCode));
            }
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
        private readonly string _stateCode;

        internal PriceCalculatorAsPerStateCode(string stateCode) : base()
        {
            this._stateCode = stateCode;
        }

        public override decimal GetTotalPrice(int itemCount, decimal itemPrice)
        {
            var totalPrice = base.GetTotalPrice(itemCount, itemPrice);
            var taxRate = 8.25m;
            return totalPrice * (1 + taxRate / 100);
        }
    }
}
