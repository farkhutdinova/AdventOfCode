using System.Buffers;

namespace dotnet;

internal static class Helper
{
    public static char[][] ReadToGrid(string inputPath)
    {
        using var reader = new StreamReader(inputPath);

        List<char[]> data = [];

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!.ToCharArray();
            data.Add(line);
        }

        return data.ToArray();
    }

    public static HashSet<Tuple<T, T>> FindPairs<T>(IList<T> items)
    {
        var pairs = new HashSet<Tuple<T, T>>();
        for (var i = 0; i < items.Count; i++)
        {
            for (var j = i + 1; j < items.Count; j++)
            {
                pairs.Add(new Tuple<T, T>(items[i], items[j]));
            }
        }
        return pairs;
    }

    public static bool WithinRange(Coordinate coordinate, int maxRow, int maxColumn)
    {
        return coordinate.Row < maxRow && coordinate.Row >= 0 &&
               coordinate.Column < maxColumn && coordinate.Column >= 0;
    }

    public static char[][] ReadToGridAndFindValue(string inputPath, SearchValues<char> search, out Coordinate index)
    {
        var foundIndex = false;
        index = default;
        using var reader = new StreamReader(inputPath);

        List<char[]> data = [];
        var i = 0;

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!.ToCharArray();
            data.Add(line);

            if (!foundIndex)
            {
                var found = line.AsSpan().IndexOfAny(search);
                if (found != -1)
                {
                    index = new Coordinate(i, found);
                    foundIndex = true;
                }
            }

            i++;
        }

        return data.ToArray();
    }
}

internal record struct Coordinate(int Row, int Column)
{
    public Coordinate Move(Direction direction)
    {
        return new Coordinate(Row + direction.Horizontal, Column + direction.Vertical);
    }
}

internal static class ArrayExtensions
{
    public static char At(this char[][] array, Coordinate coordinate) => array[coordinate.Row][coordinate.Column];
}

internal record Direction(int Horizontal, int Vertical)
{
    public static Direction From(char c)
    {
        return c switch
        {
            '^' => Directions.Up,
            '<' => Directions.Left,
            'v' => Directions.Down,
            '>' => Directions.Right
        };
    }
}

internal static class DirectionExtensions
{
    public static Direction TurnRight(this Direction current)
    {
        if (current == Directions.Up)
            return Directions.Right;
        if (current == Directions.Right)
            return Directions.Down;
        if (current == Directions.Down)
            return Directions.Left;
        if (current == Directions.Left)
            return Directions.Up;
        throw new ArgumentException(nameof(current));
    }
}

internal static class Directions
{
    public static readonly Direction Up = new(-1, 0);

    public static readonly Direction Down = new(1, 0);

    public static readonly Direction Left = new(0, -1);

    public static readonly Direction Right = new(0, 1);
}