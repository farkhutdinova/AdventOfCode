namespace dotnet._2024;

internal sealed class Day07() : DayBase(7)
{
    protected override string SolveTask1(string inputPath)
    {
        var validResults = 0L;

        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var (testResult, numbers) = ParseLine(line!);
            var combinations = GetCombinations(numbers.Length - 1);
            foreach (var combination in combinations)
            {
                var result = 0L;
                for (var i = 0; i < numbers.Length; i++)
                {
                    result = ApplyCommand(result, numbers[i], i - 1 < 0 ? null : combination.Single(c => c.Index == i  - 1).Command);
                }

                if (result == testResult)
                {
                    // Console.WriteLine("Test value " + testResult);
                    // Console.WriteLine("Result " + result);
                    // foreach (var c in combination)
                    //     Console.Write($"{c.Command} ");
                    // Console.WriteLine();

                    validResults += result;
                    break;
                }
            }
        }
        
        return validResults.ToString();
    }

    private static long ApplyCommand(long prevResult, long number, Command? command)
    {
        if (command is null) return number;

        return command switch
        {
            Command.Add => prevResult + number,
            Command.Multiply => prevResult * number,
            Command.Concatenate => long.Parse(prevResult.ToString() + number),
        };
    }

    private static (long TestResult, int[] Numbers) ParseLine(string line)
    {
        var parts = line.Split(':');
        var testResult = long.Parse(parts[0]);
        var numbers = Array.ConvertAll(parts[1].Split(' ').Skip(1).ToArray(), int.Parse);
        return (testResult, numbers);
    }

    private static List<List<(int Index, Command Command)>> GetCombinations(int amount, bool includeConcatenation = false)
    {
        var indices = Enumerable.Range(0, amount).ToArray();
        Command[] commands = includeConcatenation
            ? [Command.Add, Command.Multiply, Command.Concatenate]
            : [Command.Add, Command.Multiply];

        var cartesianProduct = from i in indices
            from c in commands
            select (i, c);

        var groups = cartesianProduct
            .GroupBy(pair => pair.i)
            .Select(group => group.ToList())
            .ToList();

        var combinations = Helper.FindCombinations(groups, 0, []);

        return combinations;
    }

    protected override string SolveTask2(string inputPath)
    {
        var validResults = 0L;

        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var (testResult, numbers) = ParseLine(line!);
            var combinations = GetCombinations(numbers.Length - 1, true);
            foreach (var combination in combinations)
            {
                var result = 0L;
                for (var i = 0; i < numbers.Length; i++)
                {
                    result = ApplyCommand(result, numbers[i], i - 1 < 0 ? null : combination.Single(c => c.Index == i  - 1).Command);
                }

                if (result == testResult)
                {
                    validResults += result;
                    break;
                }
            }
        }
        
        return validResults.ToString();
    }
}

internal enum Command
{
    Add,
    Multiply,
    Concatenate,
}