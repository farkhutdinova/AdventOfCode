using System.Buffers;

namespace dotnet._2024;

internal sealed class Day06() : DayBase(6)
{
    private static readonly char[] GuardDirections = ['v', '<', '^', '>'];
    // ReSharper disable once InconsistentNaming
    private static readonly SearchValues<char> GuardDirectionsSV = SearchValues.Create(GuardDirections);

    protected override string SolveTask1(string inputPath)
    {
        var map = Helper.ReadToGridAndFindValue(inputPath, GuardDirectionsSV, out var startPosition);
        var m = map[0].Length;
        var n = map.Length;

        var visitedCells = new HashSet<Coordinate>();
        var currentPosition = startPosition;
        var currentDirection = map.At(currentPosition);

        while (InsideMap(currentPosition, n, m))
        {
            visitedCells.Add(currentPosition);

            var triedDirections = 0;
            while (triedDirections < 4)
            {
                var next = Move(currentPosition, currentDirection);
                if (!InsideMap(next, n, m))
                {
                    currentPosition = next;
                    break;
                }
                if (map.At(next) != '#')
                {
                    currentPosition = next;
                    break;
                }

                currentDirection = TurnRight(currentDirection);
                triedDirections++;
            }
        }

        var count = visitedCells.Count;

        return count.ToString();
    }

    protected override string SolveTask2(string inputPath)
    {
        var map = Helper.ReadToGridAndFindValue(inputPath, GuardDirectionsSV, out var startPosition);
        var m = map[0].Length;
        var n = map.Length;

        var visitedCells = new HashSet<(Coordinate, Direction)>();
        var possibleObstacles = new HashSet<Coordinate>();
        var currentPosition = startPosition;
        var currentDirection = Direction.From(map.At(currentPosition));

        while (InsideMap(currentPosition, n, m))
        {
            visitedCells.Add(new (currentPosition, currentDirection));

            var triedDirections = 0;
            while (triedDirections < 4)
            {
                var next = currentPosition.Move(currentDirection);
                if (!InsideMap(next, n, m))
                {
                    currentPosition = next;
                    break;
                }
                if (map.At(next) != '#')
                {
                    currentPosition = next;
                    break;
                }

                currentDirection = currentDirection.TurnRight();
                triedDirections++;
            }
        }

        foreach (var possibleObstacle in visitedCells)
        {
            if (Check(possibleObstacle))
            {
                possibleObstacles.Add(possibleObstacle.Item1);
            }
        }

        var count = possibleObstacles.Count;

        return count.ToString();

        bool Check((Coordinate, Direction) possibleObstacle)
        {
            var direction = Directions.Up;
            var location = startPosition;
            var route = new HashSet<(Coordinate, Direction)> { (location, direction) };

            var loop = false;

            do
            {
                var next = location.Move(direction);

                if (!InsideMap(next, n, m))
                {
                    break;
                }

                if (map.At(next) == '#' || next == possibleObstacle.Item1)
                {
                    direction = direction.TurnRight();
                }
                else
                {
                    if (!route.Add((next, direction)))
                    {
                        loop = true;
                        break;
                    }

                    location = next;
                }
            }
            while (true);

            return loop;
        }
    }

    private static Coordinate Move(Coordinate currentPosition, char direction)
    {
        return direction switch
        {
            'v' => currentPosition with { Row = currentPosition.Row + 1 },
            '^' => currentPosition with { Row = currentPosition.Row - 1 },
            '>' => currentPosition with { Column = currentPosition.Column + 1 },
            '<' => currentPosition with { Column = currentPosition.Column - 1 }
        };
    }

    private static char TurnRight(char direction)
    {
        return direction switch
        {
            'v' => '<',
            '^' => '>',
            '>' => 'v',
            '<' => '^'
        };
    }

    private static bool InsideMap(Coordinate position, int n, int m)
    {
        return position.Row >= 0 && position.Column >= 0 &&
               position.Row < n && position.Column < m;
    }
}