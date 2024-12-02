namespace dotnet._2024;

internal sealed class Day02() : DayBase(2)
{
    protected override string SolveTask1(string inputPath)
    {
        var safeCount = 0;

        using var reader = new StreamReader(inputPath);
        var line = reader.ReadLine();
        while(line != null)
        {
            var numbers = line.Split(' ').Select(int.Parse).ToList();

            var sign = numbers[0] - numbers[1];

            var safe = numbers.Zip(numbers.Skip(1), (a, b) => (a - b) * sign > 0 && 0 < Math.Abs(a - b) && Math.Abs(a - b) < 4)
                .All(b => b);

            if (safe) safeCount++;

            line = reader.ReadLine();
        }

        return safeCount.ToString();
    }

    protected override string SolveTask2(string inputPath)
    {
        var safeCount = 0;

        using var reader = new StreamReader(inputPath);
        var line = reader.ReadLine();
        while(line != null)
        {
            var numbers = line.Split(' ').Select(int.Parse).ToList();

            var sign = numbers[0] - numbers[1];
            var safe = numbers
                .Zip(numbers.Skip(1), (a, b) => (a - b) * sign > 0 && 0 < Math.Abs(a - b) && Math.Abs(a - b) < 4)
                .All(b => b);

            if (safe) safeCount++;
            else
            {
                var safeWithoutOneLevel = false;
                for (var i = 0; i < numbers.Count; i++)
                {
                    var newNumbers = new List<int>(numbers);
                    newNumbers.RemoveAt(i);
                    sign = newNumbers[0] - newNumbers[1];
                    var newSafe = newNumbers
                        .Zip(newNumbers.Skip(1), (a, b) => (a - b) * sign > 0 && 0 < Math.Abs(a - b) && Math.Abs(a - b) < 4)
                        .All(b => b);
                    if (!newSafe) continue;
                    safeWithoutOneLevel = true;
                    break;
                }

                if (safeWithoutOneLevel) safeCount++;
            }

            line = reader.ReadLine();
        }

        return safeCount.ToString();
    }
}