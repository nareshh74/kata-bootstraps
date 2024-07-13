using Xunit;

namespace BowlingGameTests
{
    public class Gametests
    {
        [Fact]
        public void Game_Should_Have_10_Frames()
        {
            var game = new Game();
            Assert.Equal(10, game.Frames.Length);
        }
    }

}