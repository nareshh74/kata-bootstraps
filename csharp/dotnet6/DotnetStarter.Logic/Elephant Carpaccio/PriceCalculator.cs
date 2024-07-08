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
}
