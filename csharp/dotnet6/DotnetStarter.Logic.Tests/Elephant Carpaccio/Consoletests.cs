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
    }
}