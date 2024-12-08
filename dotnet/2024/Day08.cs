namespace dotnet._2024;

internal sealed class Day08(): DayBase(8)
{
    protected override string SolveTask1(string inputPath)
    {
        var distinctAntinodes = new HashSet<Coordinate>();
        var (antennasByFrequencies, mapWidth, mapHeight) = ScanMap(inputPath);

        foreach (var (_, coordinates) in antennasByFrequencies)
        {
            var antinodes = GetAntinodes(coordinates, mapWidth, mapHeight);
            foreach (var antinode in antinodes)
            {
                distinctAntinodes.Add(antinode);
            }
        }
        return distinctAntinodes.Count.ToString();
    }

    private static HashSet<Coordinate> GetAntinodes(HashSet<Coordinate> antennas, int mapWidth, int mapHeight)
    {
        var antinodes = new HashSet<Coordinate>();

        var pairs = Helper.FindPairs(antennas.ToList());

        foreach (var pair in pairs)
        {
            Coordinate a1, a2;
            if (pair.Item1.Row == pair.Item2.Row || pair.Item1.Column == pair.Item2.Column)
            {
                continue;
            }
            if (pair.Item1.Row > pair.Item2.Row)
            {
                a1 = pair.Item1;
                a2 = pair.Item2;
            }
            else
            {
                a1 = pair.Item2;
                a2 = pair.Item1;
            }
            var rowDiff = a1.Row - a2.Row;
            var columnDiff = a1.Column - a2.Column;

            var antinode1 = new Coordinate(a1.Row + rowDiff, a1.Column + columnDiff);
            var antinode2 = new Coordinate(a2.Row - rowDiff, a2.Column - columnDiff);

            if (Helper.WithinRange(antinode1, mapHeight, mapWidth)) antinodes.Add(antinode1);
            if (Helper.WithinRange(antinode2, mapHeight, mapWidth)) antinodes.Add(antinode2);
        }

        return antinodes;
    }
    
    private static (Dictionary<char, HashSet<Coordinate>> antennasByFrequencies, int n, int m) ScanMap(string inputPath)
    {
        var antennasByFrequencies = new Dictionary<char, HashSet<Coordinate>>();

        using var reader = new StreamReader(inputPath);

        List<char[]> data = [];

        var j = 0;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!.ToCharArray();
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '.') continue;
                if (antennasByFrequencies.TryGetValue(c, out _))
                    antennasByFrequencies[c].Add(new Coordinate(j, i));
                else
                    antennasByFrequencies.Add(c, [new Coordinate(j, i)]);
            }
            data.Add(line);
            j++;
        }

        return (antennasByFrequencies, data[0].Length, data.Count);
    }
    
    protected override string SolveTask2(string inputPath)
    {
        var distinctAntinodes = new HashSet<Coordinate>();
        var (antennasByFrequencies, mapWidth, mapHeight) = ScanMap(inputPath);

        foreach (var (_, coordinates) in antennasByFrequencies)
        {
            var antinodes = GetHarmonicAntinodes(coordinates, mapWidth, mapHeight);
            foreach (var antinode in antinodes)
            {
                distinctAntinodes.Add(antinode);
            }
        }
        return distinctAntinodes.Count.ToString();
    }
    
    private static HashSet<Coordinate> GetHarmonicAntinodes(HashSet<Coordinate> antennas, int mapWidth, int mapHeight)
    {
        var antinodes = new HashSet<Coordinate>();

        var pairs = Helper.FindPairs(antennas.ToList());

        foreach (var pair in pairs)
        {
            Coordinate a1, a2;
            if (pair.Item1.Row == pair.Item2.Row || pair.Item1.Column == pair.Item2.Column)
            {
                continue;
            }
            if (pair.Item1.Row < pair.Item2.Row)
            {
                a1 = pair.Item1;
                a2 = pair.Item2;
            }
            else
            {
                a1 = pair.Item2;
                a2 = pair.Item1;
            }
            // Console.WriteLine($"a1 = ({a1.Row}, {a1.Column}), a2 = ({a2.Row}, {a2.Column})");
            var rowDiff = a1.Row - a2.Row;
            var columnDiff = a1.Column - a2.Column;

            antinodes.Add(a1);
            antinodes.Add(a2);

            var koeff = 1;
            var antinode1 = new Coordinate(a1.Row + rowDiff, a1.Column + columnDiff);
            while (Helper.WithinRange(antinode1, mapHeight, mapWidth))
            {
                antinodes.Add(antinode1);
                // Console.WriteLine($"({antinode1.Row}, {antinode1.Column})");

                koeff++;
                antinode1 = new Coordinate(a1.Row + koeff * rowDiff, a1.Column + koeff * columnDiff);
            }

            koeff = 1;
            var antinode2 = new Coordinate(a2.Row - rowDiff, a2.Column - columnDiff);
            while (Helper.WithinRange(antinode2, mapHeight, mapWidth))
            {
                antinodes.Add(antinode2);
                // Console.WriteLine($"({antinode2.Row}, {antinode2.Column})");
                koeff++;
                antinode2 = new Coordinate(a2.Row - koeff * rowDiff, a2.Column - koeff * columnDiff);
            }
        }

        return antinodes;
    }
}