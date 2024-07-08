using System;
using System.IO;
using Xunit;
using DotnetStarter.Logic.ElephantCarpaccio;

namespace ConsoleTests
{
    public class ConsoleTests
    {
        [Fact]
        public void ShouldWelcomeUser()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.WelcomeUser();

            // Assert
            Assert.Equal("Welcome User!\r\n", writer.ToString());
        }

        [Fact]
        public void ShouldGetItemCountAsInputFromUser()
        {
            // Arrange
            var reader = new StringReader("5");
            Console.SetIn(reader);

            // Act
            var result = Program.GetItemCount();

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void ShouldOutputItemCountWhichWasGivenAsInput()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.OutputItemCount(5);

            // Assert
            Assert.Equal("You have entered 5 items.\r\n", writer.ToString());
        }

        [Fact]
        public void ShouldGetItemPriceAsInputFromUser()
        {
            // Arrange
            var reader = new StringReader("5");
            Console.SetIn(reader);

            // Act
            var result = Program.GetItemPrice();

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void ShouldGetStateCodeAsInputFromUser()
        {
            // Arrange
            var reader = new StringReader("CA");
            Console.SetIn(reader);

            // Act
            var result = Program.GetStateCode();

            // Assert
            Assert.Equal("CA", result);
        }

        [Fact]
        public void ShouldOutputStateCodeWhichWasGivenAsInput()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.OutputStateCode("CA");

            // Assert
            Assert.Equal("The state code is CA.\r\n", writer.ToString());
        }

        [Fact]
        public void ShouldOutputItemPriceWhichWasGivenAsInput()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.OutputItemPrice(5);

            // Assert
            Assert.Equal("The price of the item is 5.\r\n", writer.ToString());
        }

        [Fact]
        public void ShouldOutputTotalPrice()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.OutputTotalPrice(5, 5);

            // Assert
            Assert.Equal("The total price is 25.75.\r\n", writer.ToString());
        }

        [Fact]
        public void ShouldOutputTotalPriceWithThreePercentTax()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.OutputTotalPrice(5, 5);

            // Assert
            Assert.Equal("The total price is 25.75.\r\n", writer.ToString());
        }

        [Fact (Skip = "old behaviour")]
        public void ShouldOutputTotalPriceForCAStateCode()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.OutputTotalPriceV2(5, 5, "CA");

            // Assert
            Assert.Equal("The total price is 27.06.\r\n", writer.ToString());
        }

        [Fact]
        public void ShouldThrowExceptionForInvalidStateCode()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            var exception = Record.Exception(() => Program.OutputTotalPriceV2(5, 5, "TA"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("Invalid state code (Parameter 'stateCode')", exception.Message);
        }

        [Theory]
        [InlineData("CA", 27.0625)]
        [InlineData("AL", 26)]
        [InlineData("TX", 26.5625)]
        [InlineData("NV", 27)]
        [InlineData("UT", 26.71)] // Updated expectedTotalPrice to 26.71
        public void ShouldOutputTotalPriceForAllExpectedStateCodes(string stateCode, decimal expectedTotalPrice)
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.OutputTotalPriceV2(5, 5, stateCode);

            // Assert
            Assert.Equal($"The total price is {expectedTotalPrice:F2}.\r\n", writer.ToString()); // Updated assertion to format expectedTotalPrice with 2 decimal places
        }
    }
}