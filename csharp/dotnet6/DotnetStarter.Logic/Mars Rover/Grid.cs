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