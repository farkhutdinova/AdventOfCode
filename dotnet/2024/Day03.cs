using System.Text;
using System.Text.RegularExpressions;

namespace dotnet._2024;

internal sealed class Day03() : DayBase(3)
{
    protected override string SolveTask1(string inputPath)
    {
        var valids = new List<string>();
        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var buffer = new StringBuilder();
            var state = 0;

            while (reader.Peek() >= 0)
            {
                var ch = (char)reader.Read();
                buffer.Append(ch);

                switch (state)
                {
                    case 0:
                        if (ch == 'm') state = 1;
                        else buffer.Clear();
                        break;
                    case 1:
                        if (ch == 'u') state = 2;
                        else
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        break;
                    case 2:
                        if (ch == 'l') state = 3;
                        else
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        break;
                    case 3:
                        if (ch == '(') state = 4;
                        else
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        break;
                    case 4:
                        if (!char.IsDigit(ch) && ch != ',' && ch != ')')
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        if (ch == ')')
                        {
                            var sequence = buffer.ToString();
                            if (sequence.StartsWith("mul(") && sequence.EndsWith(")"))
                            {
                                valids.Add(sequence);
                            }

                            state = 0;
                            buffer.Clear();
                        }

                        break;
                }
            }
        }

        var sum = valids.Sum(Parse);

        return sum.ToString();
    }

    private static int Parse(string input)
    {
        var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

        var match = Regex.Match(input, pattern);
        if (!match.Success) return 0;
        var x = int.Parse(match.Groups[1].Value);
        var y = int.Parse(match.Groups[2].Value);

        return x * y;
    }

    protected override string SolveTask2(string inputPath)
    {
        var disablers = GetCommands(inputPath, @"(don't\(\))");
        var enablers = GetCommands(inputPath, @"(do\(\))");

        var disabledIntervals = ConstructIntervals(enablers, disablers);

        var i = 0;
        var valids = new List<string>();
        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var buffer = new StringBuilder();
            var state = 0;

            while (reader.Peek() >= 0)
            {
                var ch = (char)reader.Read();
                buffer.Append(ch);

                switch (state)
                {
                    case 0:
                        if (ch == 'm') state = 1;
                        else buffer.Clear();
                        break;
                    case 1:
                        if (ch == 'u') state = 2;
                        else
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        break;
                    case 2:
                        if (ch == 'l') state = 3;
                        else
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        break;
                    case 3:
                        if (ch == '(') state = 4;
                        else
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        break;
                    case 4:
                        if (!char.IsDigit(ch) && ch != ',' && ch != ')')
                        {
                            state = 0;
                            buffer.Clear();
                        }

                        if (ch == ')')
                        {
                            var sequence = buffer.ToString();
                            if (sequence.StartsWith("mul(") && sequence.EndsWith(")"))
                            {
                                if (disabledIntervals.Any(x => x.Item1 <= i && i <= x.Item2))
                                {
                                    i++;
                                    continue;
                                }

                                valids.Add(sequence);
                            }

                            state = 0;
                            buffer.Clear();
                        }

                        break;
                }
                i++;
            }
        }

        var sum = valids.Sum(Parse);

        return sum.ToString();
    }

    private static List<int> GetCommands(string inputPath, string pattern)
    {
        using var reader2 = new StreamReader(inputPath);

        var i = 0;
        var commands = new List<int>();
        while (reader2.ReadLine() is { } line)
        {
            var matches = Regex.Matches(line, pattern);
            foreach (Match match in matches)
            {
                commands.Add(i + match.Index);
            }
            i += line.Length + Environment.NewLine.Length;
        }
        return commands;
    }

    private static List<Tuple<int, int>> ConstructIntervals(List<int> enablers, List<int> disablers)
    {
        var intervals = new List<Tuple<int, int>>();
        foreach (var start in disablers)
        {
            var candidates = enablers.Where(x => x > start).ToList();
            var end = candidates.Count != 0 ? candidates.Min() : int.MaxValue;
            intervals.Add(new Tuple<int, int>(start, end));
        }

        return intervals;
    }
}