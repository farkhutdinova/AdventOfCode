namespace dotnet._2024;

internal sealed class Day01() : DayBase(1)
{
    protected override string SolveTask1(string inputPath)
    {
        var (list1, list2) = ReadLists(inputPath);

        var orderedList1 = list1.OrderBy(x => x).ToList();
        var orderedList2 = list2.OrderBy(x => x).ToList();

        var sum = orderedList1.Select((t, i) => Math.Abs(t - orderedList2[i])).Sum();

        return sum.ToString();
    }

    protected override string SolveTask2(string inputPath)
    {
        var (list1, list2) = ReadLists(inputPath);

        var disctincts = list1.CountBy(x => x);

        var sum = 0;

        foreach (var (id, count) in disctincts)
        {
            var rate = list2.CountBy(x => x == id).Where(x => x.Key).Select(x => x.Value).SingleOrDefault();
            sum += id * rate * count;
        }

        return sum.ToString();
    }

    private Tuple<List<int>, List<int>> ReadLists(string inputPath)
    {
        var list1 = new List<int>();
        var list2 = new List<int>();

        using var reader = new StreamReader(inputPath);
        var line = reader.ReadLine();
        while(line != null)
        {
            var numbers = line.Split("   ");
            list1.Add(int.Parse(numbers[0]));
            list2.Add(int.Parse(numbers[1]));
            line = reader.ReadLine();
        }

        return new Tuple<List<int>, List<int>>(list1, list2);
    }
}