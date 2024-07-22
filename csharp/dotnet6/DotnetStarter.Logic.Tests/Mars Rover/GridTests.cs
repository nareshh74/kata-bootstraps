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

    public class AddRover
    {
        [Fact]
        public void Should_add_rover_to_grid()
        {
            var grid = new Grid(5, 5);
            var position = new Position(1, 2, 'N');
            var rover = new Rover(position);

            grid.AddRover(rover);

            Assert.Single(grid.Rovers);
            Assert.Contains(rover, grid.Rovers);
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