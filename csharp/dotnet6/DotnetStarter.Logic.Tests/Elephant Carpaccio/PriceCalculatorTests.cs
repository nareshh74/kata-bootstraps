using DotnetStarter.Logic.ElephantCarpaccio;
using Xunit;

namespace DotnetStarter.Logic.Tests.Elephant_Carpaccio
{
    public class PriceCalculatorTests
    {
        [Fact (Skip = "old behaviour")]
        public void ShouldReturnTotalPrice()
        {
            // Arrange
            var itemCount = 5;
            var itemPrice = 5;
            var expectedTotalPrice = 25;
            var calculator = PriceCalculator.Create();

            // Act
            var result = calculator.GetTotalPrice(itemCount, itemPrice);

            // Assert
            Assert.Equal(expectedTotalPrice, result);
        }

        [Fact]
        public void ShouldReturnTotalPriceWithThreePercentTax()
        {
            // Arrange
            var itemCount = 5;
            var itemPrice = 5;
            var expectedTotalPrice = 25.75m;
            var calculator = PriceCalculator.Create();

            // Act
            var result = calculator.GetTotalPrice(itemCount, itemPrice);

            // Assert
            Assert.Equal(expectedTotalPrice, result);
        }

        [Fact]
        public void ShouldReturnTotalPriceForCAStateCode()
        {
            // Arrange
            var itemCount = 5;
            var itemPrice = 5;
            var stateCode = "CA";
            var expectedTotalPrice = 27.0625m;
            var calculator = PriceCalculator.Create(stateCode);

            // Act
            var result = calculator.GetTotalPrice(itemCount, itemPrice);

            // Assert
            Assert.Equal(expectedTotalPrice, result);
        }
    }
}
