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

    public static List<long> ReadToList(string inputPath)
    {
        var result = new List<long>();
        using var reader = new StreamReader(inputPath);
        var data = reader.ReadToEnd().AsSpan();
        var tokens = data.Split(' ');
        while (tokens.MoveNext())
        {
            var stone = long.Parse(data[tokens.Current]);
            result.Add(stone);
        }

        return result;
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

    public static List<List<T>> FindCombinations<T>(List<List<T>> groups, int depth, List<T> current)
    {
        var combinations = new List<List<T>>();
        if (depth == groups.Count)
        {
            combinations.Add([..current]);
            return combinations;
        }

        foreach (var item in groups[depth])
        {
            current.Add(item);
            combinations.AddRange(FindCombinations(groups, depth + 1, current));
            current.RemoveAt(current.Count - 1);
        }
        
        return combinations;
    }

    public static bool WithinRange(this Coordinate coordinate, int maxRow, int maxColumn)
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

internal readonly record struct Point(int X, int Y);

internal readonly record struct Coordinate(int Row, int Column)
{
    public Coordinate Move(Direction direction)
    {
        return new Coordinate(Row + direction.Horizontal, Column + direction.Vertical);
    }
}

internal static class ArrayExtensions
{
    public static T At<T>(this T[][] array, Coordinate coordinate) => array[coordinate.Row][coordinate.Column];
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

internal static class LongExtensions
{
    public static bool HasEvenDigits(this long stone, out (string, string) parts)
    {
        var digits = stone.ToString().AsSpan();
        if (digits.Length % 2 == 0)
        {
            var middle = digits.Length / 2;
            var part1 = digits[..middle];
            var part2 = digits[middle..];
            parts = (part1.ToString(), part2.ToString());
            return true;
        }
        parts = default;
        return false;
    }

    public static long DigitsCount(this long value) => (int)Math.Log10(value) + 1;
}