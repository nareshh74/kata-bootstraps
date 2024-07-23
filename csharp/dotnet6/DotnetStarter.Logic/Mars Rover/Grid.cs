using System.Collections.Generic;

namespace MarsRover;

public class Grid
{
    private readonly List<Rover> _rovers;
    public int Width { get; }
    public int Height { get; }
    public IReadOnlyList<Rover> Rovers => this._rovers.AsReadOnly();

    public Grid(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this._rovers = new List<Rover>();
    }

    public void AddRover(Rover rover)
    {
        this._rovers.Add(rover);
    }

    public bool IsValid(Position position)
    {
        int x = position.X, y = position.Y;
        var direction = position.Direction;

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
}

public class Rover
{
    public Position Position { get; }

    public Rover(Position position)
    {
        this.Position = position;
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