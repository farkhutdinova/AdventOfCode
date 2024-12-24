using System.Text.RegularExpressions;

namespace dotnet._2024;

internal partial class Day13() : DayBase(13)
{
    [GeneratedRegex(@"X\+(\d+), Y\+(\d+)")]
    private static partial Regex ButtonRegex { get; }
    
    [GeneratedRegex(@"X\=(\d+), Y\=(\d+)")]
    private static partial Regex PrizeRegex { get; }

    protected override string SolveTask1(string inputPath)
    {
        var tokens = 0L;
        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var line1 = reader.ReadLine();
            var line2 = reader.ReadLine();
            var line3 = reader.ReadLine();

            var clawMachine = ParseClawMachine(line1!, line2!, line3!);

            if (CanGetPrize(clawMachine, out var spentTokens))
            {
                tokens += spentTokens;
            }

            reader.ReadLine();
        }
        
        return tokens.ToString();
    }

    private static bool CanGetPrize(ClawMachine clawMachine, out long tokens)
    {
        tokens = 0;

        var p = clawMachine.Prize;
        var a = clawMachine.A;
        var b = clawMachine.B;

        if ((double)(p.X * a.Y - a.X * p.Y) % (a.X * b.X + b.X * a.Y - a.X * b.X - a.X * b.Y) == 0)
        {
            var countB = (p.X * a.Y - a.X * p.Y) / (a.X * b.X + b.X * a.Y - a.X * b.X - a.X * b.Y);
            if ((double)(p.X + p.Y - b.X * countB - b.Y * countB) % (a.X + a.Y) == 0)
            {
                var countA = (p.X + p.Y - b.X * countB - b.Y * countB) / (a.X + a.Y);
                tokens = 3 * countA + countB;
                return true; // countA <= 100  && countB <= 100;
            }
        }

        return false;
    }

    private static ClawMachine ParseClawMachine(string buttonA, string buttonB, string prize, long prizeAddition = 0)
    {
        return new ClawMachine(Button.Parse(buttonA), Button.Parse(buttonB), Prize.Parse(prize, prizeAddition));
    }

    protected override string SolveTask2(string inputPath)
    {
        var tokens = 0L;
        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var line1 = reader.ReadLine();
            var line2 = reader.ReadLine();
            var line3 = reader.ReadLine();

            var clawMachine = ParseClawMachine(line1!, line2!, line3!, 10000000000000);

            if (CanGetPrize(clawMachine, out var spentTokens))
            {
                tokens += spentTokens;
            }

            reader.ReadLine();
        }
        
        return tokens.ToString();
    }

    private record ClawMachine(Button A, Button B, Prize Prize);

    private record Button(int X, int Y)
    {
        public static Button Parse(string line)
        {
            var match = ButtonRegex.Match(line);

            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);

            return new Button(x, y);
        }
    }

    private record Prize(long X, long Y)
    {
        public static Prize Parse(string line, long prizeAddition)
        {
            var match = PrizeRegex.Match(line);

            var x = int.Parse(match.Groups[1].Value) + prizeAddition;
            var y = int.Parse(match.Groups[2].Value) + prizeAddition;

            return new Prize(x, y);
        }
    }
}