namespace dotnet._2024;

internal sealed class Day12() : DayBase(12)
{
    private readonly Direction[] _directions = [Directions.Up, Directions.Right, Directions.Down, Directions.Left];

    protected override string SolveTask1(string inputPath)
    {
        var regions = GetRegions(inputPath);

        var price = regions.Sum(region => region.Count * region.Perimeter);

        return price.ToString();
    }

    protected override string SolveTask2(string inputPath)
    {
        var regions = GetRegions(inputPath);

        var price = regions.Sum(region => region.Count * region.GetSides());

        return price.ToString();
    }

    private List<Region> GetRegions(string inputPath)
    {
        var map = Helper.ReadToGrid(inputPath);

        var width = map[0].Length;
        var height = map.Length;

        var readMap = new bool[width, height];

        var regions = new List<Region>();

        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                if (readMap[col, row]) continue;

                var current = new Coordinate(row, col);
                var region = CalculateRegion(current);

                regions.Add(region);
            }
        }

        return regions;

        // locals
        Region CalculateRegion(Coordinate start)
        {
            var value = map.At(start);
            var region = new Region();
            region.Add(start);
            readMap[start.Column, start.Row] = true;

            foreach (var dir in _directions)
            {
                var next = start.Move(dir);
                if (next.WithinRange(height, width) == false)
                {
                    region.AddToPerimeter(dir, next);
                }
                else if (map.At(next) == value)
                {
                    if (readMap[next.Column, next.Row])
                    {
                        continue;
                    }

                    var nextRegion = CalculateRegion(next);
                    foreach (var c in nextRegion.GetCoordinates())
                    {
                        region.Add(c);
                    }
                    region.AddToPerimeter(nextRegion.PerimeterBorders);
                }
                else
                {
                    region.AddToPerimeter(dir, next);
                }
            }

            return region;
        }
    }

    private record Region
    {
        private readonly HashSet<Coordinate> _coordinates = [];

        public void Add(Coordinate coordinate)
        {
            _coordinates.Add(coordinate);
        }

        public HashSet<Coordinate> GetCoordinates() => _coordinates;

        public int Count => _coordinates.Count;

        public int Perimeter => PerimeterBorders.Count;

        public HashSet<(Direction, Coordinate)> PerimeterBorders { get; } = [];

        public void AddToPerimeter(Direction dir, Coordinate cell)
        {
            PerimeterBorders.Add((dir, cell));
        }

        public void AddToPerimeter(HashSet<(Direction, Coordinate)> otherRegionPerimeterBorders)
        {
            foreach (var otherRegion in otherRegionPerimeterBorders)
            {
                PerimeterBorders.Add(otherRegion);
            }
        }

        public int GetSides()
        {
            var horizontalBorders = PerimeterBorders.Where(x => x.Item1.Vertical == 0).OrderBy(x => x.Item2.Row).ThenBy(x => x.Item2.Column).ToList();
            var horizontalBordersGroups = horizontalBorders.GroupBy(x => x.Item2.Row).ToList();

            var horDistinctSides = 0;
            foreach (var group in horizontalBordersGroups)
            {
                var dirGroups = group.GroupBy(x => x.Item1.Horizontal).ToList();
                foreach (var dirGroup in dirGroups)
                {
                    var a = dirGroup.ToArray();
                    var prev = a[0];
                    horDistinctSides++;
                    for (var i = 1; i < a.Length; i++)
                    {
                        if (Math.Abs(a[i].Item2.Column - prev.Item2.Column) > 1)
                        {
                            horDistinctSides++;
                            prev = a[i];
                            continue;
                        }
                        prev = a[i];
                    }
                }
            }

            var verticalBorders = PerimeterBorders.Where(x => x.Item1.Horizontal == 0).OrderBy(x => x.Item2.Column).ThenBy(x => x.Item2.Row).ToList();
            var verticalBordersGroups = verticalBorders.GroupBy(x => x.Item2.Column).ToList();

            var vertDistinctSides = 0;
            foreach (var group in verticalBordersGroups)
            {
                var dirGroups = group.GroupBy(x => x.Item1.Vertical).ToList();
                foreach (var dirGroup in dirGroups)
                {
                    var a = dirGroup.ToArray();
                    var prev = a[0];
                    vertDistinctSides++;
                    for (var i = 1; i < a.Length; i++)
                    {
                        if (Math.Abs(a[i].Item2.Row - prev.Item2.Row) > 1)
                        {
                            vertDistinctSides++;
                            prev = a[i];
                            continue;
                        }

                        prev = a[i];
                    }
                }
            }

            return horDistinctSides + vertDistinctSides;
        }
    }
}