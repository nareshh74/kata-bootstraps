using DotnetStarter.Logic.ElephantCarpaccio;
using Xunit;

namespace DotnetStarter.Logic.Tests.Elephant_Carpaccio
{
    public class PriceCalculatorTests
    {
        [Fact]
        public void ShouldReturnTotalPrice()
        {
            // Arrange
            var itemCount = 5;
            var itemPrice = 5;
            var expectedTotalPrice = 25;
            var calculator = new PriceCalculator();

            // Act
            var result = calculator.GetTotalPrice(itemCount, itemPrice);

            // Assert
            Assert.Equal(expectedTotalPrice, result);
        }
    }
}
