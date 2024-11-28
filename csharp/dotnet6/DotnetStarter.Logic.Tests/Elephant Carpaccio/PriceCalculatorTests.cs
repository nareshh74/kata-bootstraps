using DotnetStarter.Logic.ElephantCarpaccio;
using Xunit;

namespace DotnetStarter.Logic.Tests.Elephant_Carpaccio
{
    public class PriceCalculatorTests
    {
        [Fact]
        public void ShouldReturnTotalPriceForCAStateCode()
        {
            // Arrange
            var itemCount = 5;
            var itemPrice = 5;
            var stateCode = "CA";
            var expectedTotalPrice = 27.06m;
            var calculator = PriceCalculator.Create(stateCode);

            // Act
            var result = calculator.GetTotalPrice(itemCount, itemPrice);

            // Assert
            Assert.Equal(expectedTotalPrice, result);
        }

        [Theory]
        [InlineData("CA", 27.06)]
        [InlineData("AL", 26.00)]
        [InlineData("TX", 26.56)]
        [InlineData("NV", 27.00)]
        [InlineData("UT", 26.71)]
        public void ShouldOutputTotalPriceForAllExpectedStateCodes(string stateCode, decimal expectedTotalPrice)
        {
            // Arrange
            var itemCount = 5;
            var itemPrice = 5;
            var calculator = PriceCalculator.Create(stateCode);

            // Act
            var result = calculator.GetTotalPrice(itemCount, itemPrice);

            // Assert
            Assert.Equal(expectedTotalPrice, result);
        }
    }
}
