using Xunit;

namespace MarsRover;

public class Gridtests
{
    public class Ctor
    {
        [Fact]
        public void Should_have_expected_size()
        {
            var grid = new Grid(5, 5);

            Assert.Equal(5, grid.Width);
            Assert.Equal(5, grid.Height);
        }
    }
}

public class RoverTests
{
    public class Ctor
    {
        [Fact]
        public void Should_have_expected_position()
        {
            var position = new Position(1, 2, 'N');
            var rover = new Rover(position);

            Assert.Equal(rover.Position, position);
        }
    }
}

public class PositionTests
{
    public class Ctor
    {
        [Fact]
        public void Should_have_expected_position()
        {
            var position = new Position(1, 2, 'N');

            Assert.Equal(1, position.X);
            Assert.Equal(2, position.Y);
            Assert.Equal('N', position.Direction);
        }
    }
}