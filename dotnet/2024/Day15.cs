namespace dotnet._2024;

internal sealed class Day15() : DayBase(15)
{
    protected override string SolveTask1(string inputPath)
    {
        var sum = 0;

        var (map, start, directions) = ParseInput(inputPath);

        foreach (var dir in directions)
        {
            (map, start) = Move(map, start, dir);
            // PrintMap(map);
            // Console.WriteLine();
        }

        var width = map.GetLength(0);
        var height = map.GetLength(1);
        for (var i = 1; i < height; i++)
        for (var j = 1; j < width; j++)
        {
            if (map[i, j] == 'O')
            {
                sum += i * 100 + j;
            }
        }
        return sum.ToString();
    }

    private static void PrintMap(char[,] map)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j]);
            }

            Console.WriteLine();
        }
    }

    private static (char[,] map, Coordinate robotPosition) Move(char[,] map, Coordinate start, Direction direction)
    {
        var currentPosition = start;

        if (direction == Directions.Down)
        {
            Coordinate? firstGood = null;
            Coordinate? firstSpace = null;
            for (var i = start.Row + 1; i < map.GetLength(0); i++)
            {
                if (map[i, start.Column] == '#') break;
                if (map[i, start.Column] == 'O' && firstGood == null) firstGood = start with { Row = i };
                if (map[i, start.Column] == '.' && firstSpace == null) firstSpace = start with { Row = i };
            }

            if (firstSpace.HasValue)
            {
                currentPosition = start with { Row = start.Row + 1 };
                if (firstGood.HasValue && firstSpace.Value.Row > firstGood.Value.Row)
                {
                    map[firstGood.Value.Row, firstGood.Value.Column] = '.';
                    map[firstSpace.Value.Row, firstSpace.Value.Column] = 'O';
                }
                map[start.Row, start.Column] = '.';
            }
        }
        else if (direction == Directions.Up)
        {
            Coordinate? firstGood = null;
            Coordinate? firstSpace = null;
            for (var i = start.Row - 1; i > 0; i--)
            {
                if (map[i, start.Column] == '#') break;
                if (map[i, start.Column] == 'O' && firstGood == null) firstGood = start with { Row = i };
                if (map[i, start.Column] == '.' && firstSpace == null) firstSpace = start with { Row = i };
            }

            if (firstSpace.HasValue)
            {
                currentPosition = start with { Row = start.Row - 1 };
                if (firstGood.HasValue && firstSpace.Value.Row < firstGood.Value.Row)
                {
                    map[firstGood.Value.Row, firstGood.Value.Column] = '.';
                    map[firstSpace.Value.Row, firstSpace.Value.Column] = 'O';
                }
                map[start.Row, start.Column] = '.';
            }
        }
        else if (direction == Directions.Right)
        {
            Coordinate? firstGood = null;
            Coordinate? firstSpace = null;
            for (var j = start.Column + 1; j < map.GetLength(1); j++)
            {
                if (map[start.Row, j] == '#') break;
                if (map[start.Row, j] == 'O' && firstGood == null) firstGood = start with { Column = j };
                if (map[start.Row, j] == '.' && firstSpace == null) firstSpace = start with { Column = j };
            }

            if (firstSpace.HasValue)
            {
                currentPosition = start with { Column = start.Column + 1 };
                if (firstGood.HasValue && firstSpace.Value.Column > firstGood.Value.Column)
                {
                    map[firstGood.Value.Row, firstGood.Value.Column] = '.';
                    map[firstSpace.Value.Row, firstSpace.Value.Column] = 'O';
                }
                map[start.Row, start.Column] = '.';
            }
        }
        else if (direction == Directions.Left)
        {
            Coordinate? firstGood = null;
            Coordinate? firstSpace = null;
            for (var j = start.Column - 1; j < map.GetLength(1); j--)
            {
                if (map[start.Row, j] == '#') break;
                if (map[start.Row, j] == 'O' && firstGood == null) firstGood = start with { Column = j };
                if (map[start.Row, j] == '.' && firstSpace == null) firstSpace = start with { Column = j };
            }

            if (firstSpace.HasValue)
            {
                currentPosition = start with { Column = start.Column - 1 };
                if (firstGood.HasValue && firstSpace.Value.Column < firstGood.Value.Column)
                {
                    map[firstGood.Value.Row, firstGood.Value.Column] = '.';
                    map[firstSpace.Value.Row, firstSpace.Value.Column] = 'O';
                }
                map[start.Row, start.Column] = '.';
            }
        }
        map[currentPosition.Row, currentPosition.Column] = '@';

        return (map, currentPosition);
    }

    private static (char[,] map, Coordinate robotPosition, List<Direction> directions) ParseInput(string inputPath)
    {
        var robotPositionFound = false;
        Coordinate robotPosition = default;
        var lines = File.ReadAllLines(inputPath);
        var map = new List<char[]>();
        var i = 0;
        var currentLine = lines[i];
        while (currentLine != string.Empty)
        {
            map.Add(currentLine.ToCharArray());
            if (!robotPositionFound)
            {
                var robotColumn = currentLine.IndexOf('@');
                if (robotColumn != -1)
                {
                    robotPosition = new Coordinate(i, robotColumn);
                    robotPositionFound = true;
                }
            }

            currentLine = lines[++i];
        }

        var directions = new List<Direction>();
        for (var j = ++i; j < lines.Length; j++)
        {
            directions.AddRange(
                lines[j].ToCharArray().Select(Direction.From).ToList());
        }

        return (map.ToArray().To2DArray(), robotPosition, directions);
    }

    protected override string SolveTask2(string inputPath)
    {
        throw new NotImplementedException();
    }
}