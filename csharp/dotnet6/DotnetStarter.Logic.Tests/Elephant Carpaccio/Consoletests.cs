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
            Assert.Equal("The total price is 25.\r\n", writer.ToString());
        }
    }
}