using System.Text;
using System.Text.RegularExpressions;

namespace dotnet._2024;

internal sealed partial class Day03() : DayBase(3)
{
    private const string MulPrefix = "mul(";
    private const string Do = "do()";

    [GeneratedRegex(@"do(n\'t)?\(\)")]
    private static partial Regex DoOrDont { get; }

    [GeneratedRegex(@"mul\([0-9]+\,[0-9]+\)")]
    private static partial Regex Mul { get; }

    protected override string SolveTask1(string inputPath)
    {
        var sum = 0;
        using var reader = new StreamReader(inputPath);

        var data = reader.ReadToEnd().AsSpan();

        foreach (var match in Mul.EnumerateMatches(data))
        {
            var s = data.Slice(match.Index + MulPrefix.Length, match.Length - MulPrefix.Length - 1);
            var tokens = s.Split(',');
            tokens.MoveNext();
            var x = int.Parse(s[tokens.Current]);
            tokens.MoveNext();
            var y = int.Parse(s[tokens.Current]);

            sum += x * y;
        }

        return sum.ToString();
    }

    protected override string SolveTask2(string inputPath)
    {
        var sum = 0;
        using var reader = new StreamReader(inputPath);

        var data = reader.ReadToEnd().AsSpan();

        var valid = new StringBuilder(data.Length);
        var enabled = true;
        var i = 0;


        while (true)
        {
            var enumerator = DoOrDont.EnumerateMatches(data, i);

            if (enumerator.MoveNext())
            {
                var match = enumerator.Current;
                if (enabled)
                {
                    valid.Append(data[i..match.Index]);
                }

                i = match.Index + match.Length;
                enabled = match.Length == Do.Length;
            }
            else
            {
                break;
            }
        }

        if (enabled)
        {
            valid.Append(data[i..]);
        }

        data = valid.ToString().AsSpan();

        foreach (var match in Mul.EnumerateMatches(data))
        {
            var s = data.Slice(match.Index + MulPrefix.Length, match.Length - MulPrefix.Length - 1);
            var tokens = s.Split(',');
            tokens.MoveNext();
            var x = int.Parse(s[tokens.Current]);
            tokens.MoveNext();
            var y = int.Parse(s[tokens.Current]);

            sum += x * y;
        }

        return sum.ToString();
    }
}