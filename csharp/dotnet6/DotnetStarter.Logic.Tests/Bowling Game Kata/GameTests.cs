using System;
using BowlingGameKata;
using Xunit;

namespace BowlingGameTests
{
    public class GameTests
    {
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
                for (int i = 0; i < 18; i++)
                {
                    game.Roll(4);
                }
                Assert.Throws<InvalidOperationException>(() => game.Roll(4));
            }
        }

        public class IsComplete
        {
            [Fact]
            public void Should_Complete_After_10_Frames()
            {
                var game = new Game();
                for (int i = 0; i < 18; i++)
                {
                    game.Roll(4);
                }
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
        }
    }

}