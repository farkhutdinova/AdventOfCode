namespace dotnet._2024;

internal sealed class Day07() : DayBase(7)
{
    protected override string SolveTask1(string inputPath)
    {
        var count = 0;
        
        using var reader = new StreamReader(inputPath);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var (testResult, numbers) = ParseLine(line!);
            var combinations = GetCombinations(numbers.Length - 1);
            foreach (var combination in combinations)
            {
                Console.WriteLine("Test value " + testResult);
                foreach (var command in combination)
                {
                    Console.WriteLine($"{command.Key}: {command.Value}");
                }
            }
        }
        
        return count.ToString();
    }

    private static (long TestResult, int[] Numbers) ParseLine(string line)
    {
        var parts = line.Split(':');
        var testResult = long.Parse(parts[0]);
        var numbers = Array.ConvertAll(parts[1].Split(' ').Skip(1).ToArray(), int.Parse);
        return (testResult, numbers);
    }

    private static HashSet<Dictionary<long, Command>> GetCombinations(int amount)
    {
        var combinations = new HashSet<Dictionary<long, Command>>(new DictionaryComparer<long, Command>());
        
        var indices = Enumerable.Range(0, amount).ToArray();
        Command[] commands = [Command.Add, Command.Multiply];

        var cartesianProduct = from i in indices
            from c in commands
            select (i, c);

        var groupedByIndex = cartesianProduct.GroupBy(pair => pair.i).ToList();

        var uniqueCombination = new Dictionary<long, Command>();

        foreach (var group in groupedByIndex)
        {
            foreach (var element in group)
            {
                uniqueCombination.Clear();

                uniqueCombination.TryAdd(element.Item1, element.Item2);
                foreach (var otherGroup in groupedByIndex.Where(g => g.Key != group.Key))
                {
                    foreach (var otherElement in otherGroup)
                    {
                        uniqueCombination.TryAdd(otherElement.Item1, otherElement.Item2);
                    }
                }
                combinations.Add(uniqueCombination.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value));
            }
        }
        
        return combinations;
    }

    protected override string SolveTask2(string inputPath)
    {
        throw new NotImplementedException();
    }
}

internal enum Command
{
    Add,
    Multiply,
}

internal class DictionaryComparer<TKey, TValue> : IEqualityComparer<Dictionary<TKey, TValue>> where TKey : notnull
{
    public bool Equals(Dictionary<TKey, TValue>? x, Dictionary<TKey, TValue>? y)
    {
        if (x == null || y == null)
            return false;

        if (x.Count != y.Count)
            return false;

        foreach (var kvp in x)
        {
            if (!y.TryGetValue(kvp.Key, out var value) || !EqualityComparer<TValue>.Default.Equals(kvp.Value, value))
                return false;
        }

        return true;
    }

    public int GetHashCode(Dictionary<TKey, TValue>? obj)
    {
        if (obj == null)
            return 0;

        int hash = 17;
        foreach (var kvp in obj)
        {
            hash = hash * 31 + EqualityComparer<TKey>.Default.GetHashCode(kvp.Key);
            hash = hash * 31 + EqualityComparer<TValue>.Default.GetHashCode(kvp.Value!);
        }

        return hash;
    }
}