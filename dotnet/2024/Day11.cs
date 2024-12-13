namespace dotnet._2024;

internal sealed class Day11() : DayBase(11)
{
    protected override string SolveTask1(string inputPath)
    {
        var stones = Helper.ReadToList(inputPath);
        var blinks = 25;
        PrintStones(stones);

        for (var b = 0; b < blinks; b++)
        {
            stones = RearrangeStones(stones);
            // PrintStones(stones);
        }

        return stones.Count.ToString();
    }

    private static List<long> RearrangeStones(List<long> stones)
    {
        var result = new List<long>();

        foreach (var stone in stones)
        {
            if (stone == 0)
                result.Add(1);
            else if (stone.HasEvenDigits(out var parts))
            {
                result.Add(long.Parse(parts.Item1));
                result.Add(long.Parse(parts.Item2));
            }
            else
            {
                result.Add(stone * 2024);
            }
        }
        
        return result;
    }

    private static void PrintStones(List<long> stones)
    {
        foreach (var stone in stones)
        {
            Console.Write(stone + " ");
        }
        Console.WriteLine();
    }

    protected override string SolveTask2(string inputPath)
    {
        var stones = Helper.ReadToList(inputPath);
        var blinks = 75;
        var cache = new Dictionary<long, long>();

        var count = stones.Sum(t => CountRearranged(t, blinks, cache));

        return count.ToString();
    }

    private static long CountRearranged(long stone, int blinks, Dictionary<long, long> cache)
    {
        var key = stone * 100 + blinks;
        if (cache.TryGetValue(key, out var countRearranged))
            return countRearranged;

        var digits = stone.DigitsCount();

        if (blinks == 1)
        {
            countRearranged = digits % 2 == 0 ? 2 : 1;
        }   
        else
        {
            if (stone == 0)
            {
                countRearranged += CountRearranged(1, blinks - 1, cache);
            }
            else if (digits % 2 == 0)
            {
                var middle = digits / 2;
                var divisor = (int)Math.Pow(10, middle);

                countRearranged = CountRearranged(stone / divisor, blinks - 1, cache);
                countRearranged += CountRearranged(stone % divisor, blinks - 1, cache);
            }
            else
            {
                countRearranged += CountRearranged(stone * 2024, blinks - 1, cache);
            }
        }

        cache.TryAdd(key, countRearranged);
        return countRearranged;
    }
}