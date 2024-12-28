using System.Text.RegularExpressions;

namespace dotnet._2024;

internal partial class Day14() : DayBase(14)
{
    [GeneratedRegex(@"p\=(\d+),(\d+)")]
    private static partial Regex PositionRegex { get; }

    [GeneratedRegex(@"v\=(-?\d+),(-?\d+)")]
    private static partial Regex VelocityRegex { get; }

    // private const int Width = 11;
    // private const int Height = 7;
    
    private const int Width = 101;
    private const int Height = 103;

    protected override string SolveTask1(string inputPath)
    {
        var q1 = new Quadrant(0, Width / 2 - 1, 0, Height / 2 - 1);
        var q2 = new Quadrant(Width / 2 + 1, Width - 1, 0, Height / 2 - 1);
        var q3 = new Quadrant(0, Width / 2 - 1, Height / 2 + 1, Height - 1);
        var q4 = new Quadrant(Width / 2 + 1, Width - 1, Height / 2 + 1, Height - 1);

        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var robot = Robot.Parse(reader.ReadLine()!);
            var endPosition = robot.Move(100);
            if (q1.Contains(endPosition.X, endPosition.Y)) q1.Add();
            else if (q2.Contains(endPosition.X, endPosition.Y)) q2.Add();
            else if (q3.Contains(endPosition.X, endPosition.Y)) q3.Add();
            else if (q4.Contains(endPosition.X, endPosition.Y)) q4.Add();
            else
            {
                Console.WriteLine($"Missed position: ({endPosition.X}, {endPosition.Y})");
            }
        }

        return (q1.Count * q2.Count * q3.Count * q4.Count).ToString();
    }

    private record Robot(int StartX, int StartY, int MoveX, int MoveY)
    {
        public static Robot Parse(string line)
        {
            var parts = line.Split(' ');
            var positionMatch = PositionRegex.Match(parts[0]);
            var startX = int.Parse(positionMatch.Groups[1].Value);
            var startY = int.Parse(positionMatch.Groups[2].Value);
            
            var velMatch = VelocityRegex.Match(parts[1]);
            var moveX = int.Parse(velMatch.Groups[1].Value);
            var moveY = int.Parse(velMatch.Groups[2].Value);
            
            return new Robot(startX, startY, moveX, moveY);
        }

        public Point Move(int count)
        {
            var newX = (StartX + MoveX * count) % Width;
            var endX = newX == 0 || MoveX >= 0 ? newX : Width + newX;
            
            var newY = (StartY + MoveY * count) % Height;
            var endY = newY == 0 || MoveY >= 0 ? newY : Height + newY;
            return new Point(endX, endY);
        }
        
        public Point Step()
        {
            var newX = StartX + MoveX;
            var endX = newX < 0 ? Width + newX : newX >= Width ? newX - Width : newX;

            var newY = StartY + MoveY;
            var endY = newY < 0 ? Height + newY : newY >= Height ? newY - Height : newY;

            return new Point(endX, endY);
        }

        public int StartY { get; set; } = StartY;
        public int StartX { get; set; } = StartX;
    }

    private record Quadrant(int StartX, int EndX, int StartY, int EndY)
    {
        public int Count { get; private set; }
        
        public bool Contains(int x, int y)
        {
            return x >= StartX && x <= EndX && y >= StartY && y <= EndY;
        }
        
        public void Add() => Count += 1;
    }

    protected override string SolveTask2(string inputPath)
    {
        var robotsList = new List<Robot>();

        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var robot = Robot.Parse(reader.ReadLine()!);
            robotsList.Add(robot);
        }

        var robots = robotsList.ToArray();
        var robotCount = robots.Length;

        var t = 0;
        while (true)
        {
            t++;
            var positions = new bool[Width, Height];

            for (var i = 0; i < robotCount; i++)
            {
                var endPosition = robots[i].Step();
                positions[endPosition.X, endPosition.Y] = true;
                robots[i].StartX = endPosition.X;
                robots[i].StartY = endPosition.Y;
            }

            if ((t - 70) % 101 == 0 || (t - 19) % 103 == 0) // 70 is time when horizontal structure first noticed, 19 is for vertical
            {
                Console.WriteLine(t);

                for (var i = 0; i < Width; i++)
                {
                    for (var j = 0; j < Height; j++)
                        Console.Write(positions[i, j] ? "#" : " ");
                    Console.WriteLine();
                }
            }
        }

        return "0";
    }
}