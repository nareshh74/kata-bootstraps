namespace DotnetStarter.Logic.ElephantCarpaccio
{
    public class PriceCalculator
    {
        public decimal GetTotalPrice(int itemCount, decimal itemPrice)
        {
            return itemCount * itemPrice;
        }
    }
}
