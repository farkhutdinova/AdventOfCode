namespace dotnet._2024;

internal sealed class Day10(): DayBase(10)
{
    private static readonly Direction[] Directions = [dotnet.Directions.Up, dotnet.Directions.Right, dotnet.Directions.Down, dotnet.Directions.Left];
    
    protected override string SolveTask1(string inputPath)
    {
        var map = Helper.ReadToGrid(inputPath)
            .Select(row => row.Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();

        var scores = 0;

        for (var row = 0; row < map.Length; row++)
        {
            for (var i = 0; i < map[0].Length; i++)
            {
                var current = new Coordinate(row, i);
                if (map.At(current) != 0)
                    continue;
                var (heads, _) = GetHeads(current, map);
                scores += heads.Count;
            }
        }

        return scores.ToString();
    }

    private static (HashSet<Coordinate>, int) GetHeads(Coordinate current, int[][] map)
    {
        var heads = new HashSet<Coordinate>();
        var rating = 0;

        foreach (var dir in Directions)
        {
            var next = current.Move(dir);

            if (!Helper.WithinRange(next, map.Length, map[0].Length))
                continue;
            if (map.At(next) - map.At(current) != 1)
                continue;
            if (map.At(next) == 9)
            {
                heads.Add(next);
                rating++;
                continue;
            }

            var (subheads, trails) = GetHeads(next, map);
            foreach (var subhead in subheads)
            {
                heads.Add(subhead);
            }
            rating += trails;
        }

        return (heads, rating);
    }

    protected override string SolveTask2(string inputPath)
    {
        var map = Helper.ReadToGrid(inputPath)
            .Select(row => row.Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();

        var rating = 0;

        for (var row = 0; row < map.Length; row++)
        {
            for (var i = 0; i < map[0].Length; i++)
            {
                var current = new Coordinate(row, i);
                if (map.At(current) != 0)
                    continue;
                var (_, trails) = GetHeads(current, map);
                rating += trails;
            }
        }

        return rating.ToString();
    }
}