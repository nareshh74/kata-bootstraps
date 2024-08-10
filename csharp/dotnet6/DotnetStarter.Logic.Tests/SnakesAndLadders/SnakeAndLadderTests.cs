using DotnetStarter.Logic.SnakesAndLadders;
using System.IO;
using System;
using Xunit;

namespace DotnetStarter.Logic.Tests.SnakesAndLadders
{
    public class SnakeAndLadderTests
    {
        [Fact]
        public void SimulationShouldWorkAsExpected()
        {
            // Arrange
            var gameBuilder = new GameBuilder();
            gameBuilder.AddBoardObject(new Snake(62, 5));
            gameBuilder.AddBoardObject(new Snake(33, 6));
            gameBuilder.AddBoardObject(new Snake(49, 9));
            gameBuilder.AddBoardObject(new Snake(88, 16));
            gameBuilder.AddBoardObject(new Snake(41, 20));
            gameBuilder.AddBoardObject(new Snake(56, 53));
            gameBuilder.AddBoardObject(new Snake(98, 64));
            gameBuilder.AddBoardObject(new Snake(93, 73));
            gameBuilder.AddBoardObject(new Snake(95, 75));
            gameBuilder.AddBoardObject(new Ladder(2, 37));
            gameBuilder.AddBoardObject(new Ladder(27, 46));
            gameBuilder.AddBoardObject(new Ladder(10, 32));
            gameBuilder.AddBoardObject(new Ladder(51, 68));
            gameBuilder.AddBoardObject(new Ladder(61, 79));
            gameBuilder.AddBoardObject(new Ladder(65, 84));
            gameBuilder.AddBoardObject(new Ladder(71, 91));
            gameBuilder.AddBoardObject(new Ladder(81, 100));
            gameBuilder.AddPlayer(new Player("Gaurav"));
            gameBuilder.AddPlayer(new Player("Sagar"));
            var game = gameBuilder.Build();
            const string expected = @"Gaurav rolled a 6 and moved from 0 to 6
Sagar rolled a 1 and moved from 0 to 1
Gaurav rolled a 6 and moved from 6 to 12
Sagar rolled a 4 and moved from 1 to 5
Gaurav rolled a 4 and moved from 12 to 16
Sagar rolled a 6 and moved from 5 to 11
Gaurav rolled a 5 and moved from 16 to 21
Sagar rolled a 4 and moved from 11 to 15
Gaurav rolled a 1 and moved from 21 to 22
Sagar rolled a 6 and moved from 15 to 21
Gaurav rolled a 6 and moved from 22 to 28
Sagar rolled a 2 and moved from 21 to 23
Gaurav rolled a 6 and moved from 28 to 34
Sagar rolled a 6 and moved from 23 to 29
Gaurav rolled a 5 and moved from 34 to 39
Sagar rolled a 2 and moved from 29 to 31
Gaurav rolled a 2 and moved from 39 to 20
Sagar rolled a 5 and moved from 31 to 36
Gaurav rolled a 3 and moved from 20 to 23
Sagar rolled a 5 and moved from 36 to 20
Gaurav rolled a 6 and moved from 23 to 29
Sagar rolled a 3 and moved from 20 to 23
Gaurav rolled a 2 and moved from 29 to 31
Sagar rolled a 3 and moved from 23 to 26
Gaurav rolled a 3 and moved from 31 to 34
Sagar rolled a 5 and moved from 26 to 31
Gaurav rolled a 3 and moved from 34 to 37
Sagar rolled a 4 and moved from 31 to 35
Gaurav rolled a 2 and moved from 37 to 39
Sagar rolled a 5 and moved from 35 to 40
Gaurav rolled a 2 and moved from 39 to 20
Sagar rolled a 5 and moved from 40 to 45
Gaurav rolled a 2 and moved from 20 to 22
Sagar rolled a 6 and moved from 45 to 68
Gaurav rolled a 3 and moved from 22 to 25
Sagar rolled a 3 and moved from 68 to 91
Gaurav rolled a 5 and moved from 25 to 30
Sagar rolled a 2 and moved from 91 to 73
Gaurav rolled a 5 and moved from 30 to 35
Sagar rolled a 6 and moved from 73 to 79
Gaurav rolled a 5 and moved from 35 to 40
Sagar rolled a 1 and moved from 79 to 80
Gaurav rolled a 4 and moved from 40 to 44
Sagar rolled a 2 and moved from 80 to 82
Gaurav rolled a 5 and moved from 44 to 9
Sagar rolled a 4 and moved from 82 to 86
Gaurav rolled a 1 and moved from 9 to 32
Sagar rolled a 6 and moved from 86 to 92
Gaurav rolled a 3 and moved from 32 to 35
Sagar rolled a 4 and moved from 92 to 96
Gaurav rolled a 1 and moved from 35 to 36
Sagar rolled a 1 and moved from 96 to 97
Gaurav rolled a 1 and moved from 36 to 37
Sagar rolled a 5 and moved from 97 to 97
Gaurav rolled a 6 and moved from 36 to 42
Sagar rolled a 3 and moved from 97 to 100
Sagar wins the game";
            using StringWriter sw = new();
            Console.SetOut(sw);

            // Act
            game.Simulate();

            // Assert
            var result = sw.ToString();
            Assert.True(expected == result.Trim(), $"Expected: {expected}, Actual: {result}");
        }
    }
}