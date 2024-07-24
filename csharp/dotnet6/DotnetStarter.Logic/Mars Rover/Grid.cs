using System.Collections.Generic;

namespace MarsRover;

public class Grid
{
    private readonly List<Rover> _rovers;
    public int Width { get; }
    public int Height { get; }
    public IReadOnlyList<Rover> Rovers => this._rovers.AsReadOnly();

    public Grid(int width, int height, List<Rover> rovers)
    {
        this.Width = width;
        this.Height = height;
        this._rovers = rovers;
    }

    public bool IsRoverInValidPositionToMove(Rover rover)
    {
        int x = rover.Position.X, y = rover.Position.Y;
        var direction = rover.Position.Direction;

        if (x < 0 || y < 0 || x >= this.Width || y >= this.Height)
        {
            return false;
        }

        if (x == 0 && direction == 'S')
        {
            return false;
        }
        if (y == 0 && direction == 'W')
        {
            return false;
        }
        if (x == this.Width - 1 && direction == 'N')
        {
            return false;
        }
        if (y == this.Height - 1 && direction == 'E')
        {
            return false;
        }

        return true;
    }

    public void MoveRover(Rover rover)
    {
        if (this.IsRoverInValidPositionToMove(rover))
        {
            rover.Move();
        }
    }
}

public class Rover
{
    public Position Position { get; private set; }

    public Rover(Position position)
    {
        this.Position = position;
    }

    internal void Move()
    {
        var x = this.Position.X;
        var y = this.Position.Y;
        var direction = this.Position.Direction;

        this.Position = direction switch
        {
            'N' => new Position(x + 1, y, direction),
            'E' => new Position(x, y + 1, direction),
            'S' => new Position(x - 1, y, direction),
            'W' => new Position(x, y - 1, direction),
            _ => this.Position
        };
    }

    public void Turn(char direction)
    {
        var x = this.Position.X;
        var y = this.Position.Y;
        var currentDirection = this.Position.Direction;

        this.Position = (currentDirection, direction) switch
        {
            ('N', 'L') => new Position(x, y, 'W'),
            ('E', 'L') => new Position(x, y, 'N'),
            ('S', 'L') => new Position(x, y, 'E'),
            ('W', 'L') => new Position(x, y, 'S'),
            ('N', 'R') => new Position(x, y, 'E'),
            ('E', 'R') => new Position(x, y, 'S'),
            ('S', 'R') => new Position(x, y, 'W'),
            ('W', 'R') => new Position(x, y, 'N'),
            _ => this.Position
        };
    }
}

public record Position
{
    public int X { get; }
    public int Y { get; }
    public char Direction { get; }

    public Position(int x, int y, char direction)
    {
        this.X = x;
        this.Y = y;
        this.Direction = direction;
    }
}