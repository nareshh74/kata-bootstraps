using System;
using System.Linq;
using BowlingGameKata;
using Xunit;

namespace BowlingGameTests
{
    public class GameTests
    {
        private static void RollMany(Game game, int rollCount, int rollValue)
        {
            for (int i = 0; i < rollCount; i++)
            {
                game.Roll(rollValue);
            }
        }

        private static void Complete_Game_With_Given_Roll_Value(Game game, int rollValue)
        {
            while (!game.IsComplete())
            {
                game.Roll(rollValue);
            }
        }

        public class Ctor
        {
            [Fact]
            public void Should_Not_Have_any_Frames()
            {
                var game = new Game();
                Assert.Equal(0, game.Frames.Count);
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
                Assert.Equal(1, game.Frames.Count);
            }

            [Fact]
            public void Should_Throw_Roll_After_Game_Completes()
            {
                var game = new Game();
                GameTests.Complete_Game_With_Given_Roll_Value(game, 4);
                Assert.Throws<InvalidOperationException>(() => game.Roll(4));
            }

            [Fact]
            public void Should_Allow_One_Extra_Roll_In_Tenth_Frame_If_Spare_Is_Thrown()
            {
                var game = new Game();
                GameTests.RollMany(game, 18, 4);
                game.Roll(5);
                game.Roll(5);
                game.Roll(5);
                Assert.Equal(10, game.Frames.Count);
                Assert.Equal(3, game.Frames.Last().Rolls.Count);
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
            [InlineData(10, 100 + 45 + 4)]
            [InlineData(1, 86)]
            [InlineData(5, 114)]
            public void Should_Return_Correct_Score_For_Spare(int spareFrameCount, int expectedScore)
            {
                var game = new Game();
                for(int i = 0; i < spareFrameCount; i++)
                {
                    game.Roll(5);
                    game.Roll(5);
                }

                while (!game.IsComplete())
                {
                    game.Roll(4);
                }

                Assert.Equal(expectedScore, game.GetScore());
            }

            [Theory]
            [InlineData(10, 100 + 160 + 14 + 8)]
            [InlineData(1, 90)]
            [InlineData(5, 50 + 60 + 14 + 8 + 40)]
            public void Should_Return_Correct_Score_For_Strike(int strikeFrameCount, int expectedScore)
            {
                var game = new Game();
                for(int i = 0; i < strikeFrameCount; i++)
                {
                    game.Roll(10);
                }
                while (!game.IsComplete())
                {
                    game.Roll(4);
                }
                Assert.Equal(expectedScore, game.GetScore());
            }
        }
    }

}