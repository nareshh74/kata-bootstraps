using System;
using System.IO;
using Xunit;

namespace ConsoleTests
{
    public class ConsoleTests
    {
        [Fact]
        public void ShouldWriteHelloWorld()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            Program.Main(new string[0]);

            // Assert
            Assert.Equal("Hello, World!\r\n", writer.ToString());
        }
    }
}