using System.Collections.Generic;
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

    public class IsValid
    {
        [Theory]
        [InlineData(5, 5, 0, 0, 'N', true)]
        [InlineData(5, 5, 0, 0, 'E', true)]
        [InlineData(5, 5, 4, 4, 'S', true)]
        [InlineData(5, 5, 4, 4, 'W', true)]
        [InlineData(5, 5, 0, 0, 'W', false)]
        [InlineData(5, 5, 0, 0, 'S', false)]
        [InlineData(5, 5, 4, 4, 'N', false)]
        [InlineData(5, 5, 4, 4, 'E', false)]
        public void Should_return_expected_valid_result_for_corner_cells(int x,
            int y,
            int posX,
            int posY,
            char direction,
            bool expected)
        {
            var grid = new Grid(x, y);
            var position = new Position(posX, posY, direction);

            var result = grid.IsValid(position);

            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData(5, 5, -1, 0, 'N', false)]
        [InlineData(5, 5, 0, -1, 'N', false)]
        [InlineData(5, 5, 5, 0, 'N', false)]
        [InlineData(5, 5, 0, 5, 'N', false)]
        public void Should_return_false_when_out_of_bounds(int x,
            int y,
            int posX,
            int posY,
            char direction,
            bool expected)
        {
            var grid = new Grid(x, y);
            var position = new Position(posX, posY, direction);

            var result = grid.IsValid(position);

            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData(5, 5, 0, 'N', true)]
        [InlineData(5, 5, 0, 'E', true)]
        [InlineData(5, 5, 0, 'W', true)]
        [InlineData(5, 5, 4, 'E', true)]
        [InlineData(5, 5, 4, 'S', true)]
        [InlineData(5, 5, 4, 'W', true)]
        [InlineData(5, 5, 0, 'S', false)]
        [InlineData(5, 5, 4, 'N', false)]
        public void Should_return_expected_valid_result_for_top_and_bottom_edge_cells(int x,
            int y,
            int edgeXIndex,
            char direction,
            bool expected)
        {
            var grid = new Grid(x, y);
            for(int i = 1; i < y - 1; i++)
            {
                var position = new Position(edgeXIndex, i, direction);
                var result = grid.IsValid(position);
                Assert.Equal(result, expected);
            }
        }

        [Theory]
        [InlineData(5, 5, 0, 'N', true)]
        [InlineData(5, 5, 0, 'E', true)]
        [InlineData(5, 5, 0, 'S', true)]
        [InlineData(5, 5, 4, 'N', true)]
        [InlineData(5, 5, 4, 'S', true)]
        [InlineData(5, 5, 4, 'W', true)]
        [InlineData(5, 5, 0, 'W', false)]
        [InlineData(5, 5, 4, 'E', false)]
        public void Should_return_expected_valid_result_for_left_and_right_edge_cells(int x,
            int y,
            int edgeYIndex,
            char direction,
            bool expected)
        {
            var grid = new Grid(x, y);
            for (int i = 1; i < x - 1; i++)
            {
                var position = new Position(i, edgeYIndex, direction);
                var result = grid.IsValid(position);
                Assert.Equal(result, expected);
            }
        }

        [Fact]
        public void Should_return_true_when_inside_grid()
        {
            var directionList = new List<char> { 'N', 'E', 'S', 'W' };
            const int x = 5;
            const int y = 5;
            var grid = new Grid(x, y);

            for(int i = 1; i < x - 1; i++)
            {
                for (int j = 1; j < y - 1; j++)
                {
                    foreach (var direction in directionList)
                    {
                        var position = new Position(i, j, direction);
                        var result = grid.IsValid(position);
                        Assert.True(result);
                    }
                }
            }
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