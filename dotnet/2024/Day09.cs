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
            if (compacted[i] == -1) break;
            checksum += i * compacted[i];
        }
        return checksum;
    }

    protected override string SolveTask2(string inputPath)
    {
        throw new NotImplementedException();
    }
}