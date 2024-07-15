using System;
using System.Linq;
using BowlingGameKata;
using Xunit;

namespace BowlingGameTests
{
    public class GameTests
    {
        private static void Complete_Game_With_Given_Roll_Value(Game game, int rollValue)
        {
            while (!game.IsComplete())
            {
                game.Roll(rollValue);
            }
        }

        private static void Complete_Frame_With_Given_Roll_Value(Game game, int rollValue)
        {
            var currentFrame = game.Frames.Last();
            do
            {
                game.Roll(rollValue);
            }
            while (!currentFrame.IsComplete());
        }

        public class Ctor
        {
            [Fact]
            public void Should_Have_atleast_1_Frame()
            {
                var game = new Game();
                Assert.Single(game.Frames);
            }
        }

        public class Roll
        {
            [Fact]
            public void Should_Start_Next_Frame_After_2_Rolls()
            {
                var game = new Game();
                game.Roll(5);
                game.Roll(4);
                Assert.Equal(2, game.Frames.Count);
            }

            [Fact]
            public void Should_Throw_Roll_After_Game_Completes()
            {
                var game = new Game();
                GameTests.Complete_Game_With_Given_Roll_Value(game, 4);
                Assert.Throws<InvalidOperationException>(() => game.Roll(4));
            }
        }

        public class IsComplete
        {
            [Fact]
            public void Should_Complete_After_10_Frames()
            {
                var game = new Game();
                GameTests.Complete_Game_With_Given_Roll_Value(game, 4);
                Assert.True(game.IsComplete());
            }
        }

        public class GetScore
        {
            [Fact]
            public void Should_Throw_If_Game_not_Complete()
            {
                var game = new Game();
                Assert.Throws<InvalidOperationException>(() => game.GetScore());
            }

            [Fact]
            public void Should_Return_Correct_Score_For_Normal_Rolls()
            {
                var game = new Game();
                GameTests.Complete_Game_With_Given_Roll_Value(game, 4);
                Assert.Equal(80, game.GetScore());
            }

            [Theory]
            [InlineData(10, 145)]
            [InlineData(1, 86)]
            [InlineData(5, 114)]
            public void Should_Return_Correct_Score_For_Spare(int spareFrameCount, int expectedScore)
            {
                var game = new Game();
                for(int i = 0; i < spareFrameCount; i++)
                {
                    GameTests.Complete_Frame_With_Given_Roll_Value(game, 5);
                }
                for(int i = 0; i < 10 - spareFrameCount; i++)
                {
                    GameTests.Complete_Frame_With_Given_Roll_Value(game, 4);
                }
                Assert.Equal(expectedScore, game.GetScore());
            }

            [Theory]
            [InlineData(10, 270)]
            [InlineData(1, 90)]
            [InlineData(5, 50 + 60 + 14 + 8 + 40)]
            public void Should_Return_Correct_Score_For_Strike(int strikeFrameCount, int expectedScore)
            {
                var game = new Game();
                for(int i = 0; i < strikeFrameCount; i++)
                {
                    GameTests.Complete_Frame_With_Given_Roll_Value(game, 10);
                }
                for(int i = 0; i < 10 - strikeFrameCount; i++)
                {
                    GameTests.Complete_Frame_With_Given_Roll_Value(game, 4);
                }
                Assert.Equal(expectedScore, game.GetScore());
            }
        }
    }

}