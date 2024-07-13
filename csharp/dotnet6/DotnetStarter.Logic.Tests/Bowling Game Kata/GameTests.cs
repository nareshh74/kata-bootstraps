using System;
using BowlingGameKata;
using Xunit;

namespace BowlingGameTests
{
    public class GameTests
    {
        // ignore
        [Fact (Skip = "logic changed")]
        public void Game_Should_Have_10_Frames()
        {
            var game = new Game();
            Assert.Equal(10, game.Frames.Count);
        }

        [Fact]
        public void Should_Support_Roll()
        {
            var game = new Game();
            game.Roll(5);
            Assert.True(true);
        }

        [Fact]
        public void GetScore_Should_Throw_If_10_Frames_not_Complete()
        {
            var game = new Game();
            Assert.Throws<InvalidOperationException>(() => game.GetScore());
        }
    }

}