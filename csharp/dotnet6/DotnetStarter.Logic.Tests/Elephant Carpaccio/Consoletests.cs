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
    }
}