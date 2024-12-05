namespace dotnet._2024;

internal sealed class Day05() : DayBase(5)
{
    protected override string SolveTask1(string inputPath)
    {
        var sum = 0;

        using var reader = new StreamReader(inputPath);
        var rules = ReadRules(reader);

        while (!reader.EndOfStream)
        {
            if (IsValid1(reader.ReadLine().AsSpan(), out var middleValue))
            {
                sum += middleValue;
            }
        }

        return sum.ToString();

        // local functions
        bool IsValid1(ReadOnlySpan<char> update, out int middleValue)
        {
            middleValue = 0;
            var tokens = update.Split(',');
            var pages = new List<int>();

            while (tokens.MoveNext())
            {
                var page = int.Parse(update[tokens.Current]);
                if (rules.TryGetValue(page, out var shouldComeAfter))
                {
                    if (pages.Any(prevPage => shouldComeAfter.Contains(prevPage)))
                        return false;
                }

                pages.Add(page);
            }

            middleValue = pages[pages.Count / 2];

            return true;
        }
    }

    protected override string SolveTask2(string inputPath)
    {
        var sum = 0;

        using var reader = new StreamReader(inputPath);
        var rules = ReadRules(reader);

        while (!reader.EndOfStream)
        {
            if (IsFixedInvalid(reader.ReadLine().AsSpan(), out var middleValue))
            {
                sum += middleValue;
            }
        }

        return sum.ToString();

        // local functions
        bool IsFixedInvalid(ReadOnlySpan<char> update, out int middleValue)
        {
            var invalid = false;
            middleValue = 0;
            var tokens = update.Split(',');
            var pages = new List<int>();

            while (tokens.MoveNext())
            {
                var newPages = new List<int>(pages);

                var page = int.Parse(update[tokens.Current]);
                if (rules.TryGetValue(page, out var shouldComeAfter))
                {
                    foreach (var prevPage in pages)
                    {
                        if (!shouldComeAfter.Contains(prevPage)) continue;

                        var indexToInsert = pages.IndexOf(prevPage);
                        if (indexToInsert == -1) continue;

                        invalid = true;
                        newPages.Insert(indexToInsert, page);
                        break;
                    }
                }
                if (pages.Count == newPages.Count)
                    pages.Add(page);
                else
                    pages = newPages;
            }

            var isItReallyFixed = IsValid(pages);
            if (isItReallyFixed)
            {
                middleValue = pages[pages.Count / 2];
            }
            else
            {
                return IsFixedInvalid(update, out middleValue);
            }

            return invalid;
        }

        bool IsValid(List<int> update)
        {
            var pages = new List<int>();

            foreach (var page in update)
            {
                if (rules.TryGetValue(page, out var shouldComeAfter))
                {
                    if (pages.Any(prevPage => shouldComeAfter.Contains(prevPage)))
                        return false;
                }

                pages.Add(page);
            }

            return true;
        }
    }

    private static Dictionary<int, List<int>> ReadRules(StreamReader reader)
    {
        var rules = new Dictionary<int, List<int>>();

        var rule = reader.ReadLine();
        while (!string.IsNullOrEmpty(rule))
        {
            var ruleParts = rule.Split('|');
            var key = int.Parse(ruleParts[0]);
            var value = int.Parse(ruleParts[1]);
            if(rules.TryGetValue(key, out var valueList))
                valueList.Add(value);
            else
                rules.Add(key, [value]);

            rule = reader.ReadLine();
        }

        return rules;
    }
}