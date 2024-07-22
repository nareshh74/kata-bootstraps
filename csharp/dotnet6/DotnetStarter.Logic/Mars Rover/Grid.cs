namespace MarsRover;

public class Grid
{
    public int Width { get; }
    public int Height { get; }

    public Grid(int width, int height)
    {
        this.Width = width;
        this.Height = height;
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