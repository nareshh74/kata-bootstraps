namespace DotnetStarter.Logic.ElephantCarpaccio
{
    public class PriceCalculator
    {
        private PriceCalculator()
        {
        }

        public static PriceCalculator Create()
        {
            return new PriceCalculator();
        }

        public decimal GetTotalPrice(int itemCount, decimal itemPrice)
        {
            return itemCount * itemPrice;
        }
    }
}
