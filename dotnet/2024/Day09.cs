namespace dotnet._2024;

internal sealed class Day09() : DayBase(9)
{
    protected override string SolveTask1(string inputPath)
    {
        using var reader = new StreamReader(inputPath);

        var diskMap = reader.ReadLine().AsSpan();

        var blocks = ParseBlock(diskMap);

        var compacted = Compact(blocks);

        var checksum = CalculateChecksum(compacted);

        return checksum.ToString();
    }

    private static int[] ParseBlock(ReadOnlySpan<char> diskMap)
    {
        var blocks = new List<int>();

        var isFile = true;
        var i = 0;
        foreach (var c in diskMap)
        {
            var count = int.Parse(c.ToString());

            if (isFile)
            {
                blocks.AddRange(Enumerable.Repeat(i, count));
                i++;
            }
            else
            {
                blocks.AddRange(Enumerable.Repeat(-1, count));
            }

            isFile = !isFile;
        }

        return blocks.ToArray();
    }

    private static int[] Compact(int[] blocks)
    {
        var length = blocks.Length;

        var i = length - 1;
        var emptyIndex = Array.IndexOf(blocks, -1);
        while (emptyIndex < i)
        {
            if (blocks[i] == -1)
            {
                i--;
                continue;
            }

            blocks[emptyIndex] = blocks[i];
            blocks[i] = -1;

            i--;
            emptyIndex =  Array.IndexOf(blocks, -1);
        }

        return blocks;
    }

    private static long CalculateChecksum(int[] compacted)
    {
        var checksum = 0L;
        for (var i = 0; i < compacted.Length; i++)
        {
            if (compacted[i] == -1) continue;
            checksum += i * compacted[i];
        }
        return checksum;
    }

    protected override string SolveTask2(string inputPath)
    {
        using var reader = new StreamReader(inputPath);

        var diskMap = reader.ReadLine().AsSpan();

        var blocks = ParseWholeBlocks(diskMap);

        var compacted = CompactByWholeBlocks(blocks);

        var checksum = CalculateChecksum(compacted.SelectMany(b => Enumerable.Repeat(b.Value, b.Length)).ToArray());

        return checksum.ToString();
    }

    private static Block[] ParseWholeBlocks(ReadOnlySpan<char> diskMap)
    {
        var blocks = new List<Block>();

        var isFile = true;
        var i = 0;
        var index = 0;
        foreach (var c in diskMap)
        {
            var count = int.Parse(c.ToString());

            if (isFile)
            {
                var fileBlock = new Block(i, count, index);
                if (fileBlock.Length > 0) blocks.Add(fileBlock);
                i++;
            }
            else
            {
                var emptyBlock = new Block(-1, count, index);
                if (emptyBlock.Length > 0) blocks.Add(emptyBlock);
            }

            isFile = !isFile;
            index++;
        }

        return blocks.ToArray();
    }

    private static Block[] CompactByWholeBlocks(Block[] blocks)
    {
        var compacted = blocks.ToList();

        var i = compacted.Count - 1;
        var emptyBlocks = blocks.TakeWhile((_, index) => index < i).Where(b => b is { Value: -1, Length: > 0 }).ToArray();
        var current = blocks[i];
        var gapToFill = emptyBlocks.FirstOrDefault(b => b.Length >= current.Length);
        while (gapToFill != null)
        {
            var currenIndex = compacted.IndexOf(current);

            if (gapToFill.Length == current.Length)
            {
                compacted[gapToFill.Index] = current with { Index = gapToFill.Index };
                compacted[current.Index] = gapToFill with { Index = current.Index };
                i--;
            }
            else
            {
                compacted[gapToFill.Index] = current;
                compacted.Insert(gapToFill.Index + 1, gapToFill with { Length = gapToFill.Length - current.Length });
                compacted[currenIndex + 1] = gapToFill with { Length = current.Length };

                for (var k = 0; k < compacted.Count; k++)
                {
                    compacted[k] = compacted[k] with { Index = k };
                }
                i = compacted.Count - 2;
            }

            emptyBlocks = compacted.TakeWhile((_, index) => index < i).Where(b => b is { Value: -1, Length: > 0 }).ToArray();

            gapToFill = null;
            for (var j = i; j > 0; j--)
            {
                if (compacted[j].Value == -1) continue;
                current = compacted[j];
                gapToFill = emptyBlocks.FirstOrDefault(b => b.Length >= current.Length);
                if (gapToFill != null) break;
            }
        }

        return compacted.ToArray();
    }
}

internal record Block(int Value, int Length, int Index);