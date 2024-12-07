namespace dotnet._2024;

public sealed class Day04() : DayBase(4)
{
    protected override string SolveTask1(string inputPath)
    {
        var grid = Helper.ReadToGrid(inputPath);

        const string word = "XMAS";

        var found = SearchWord(grid, word);

        // foreach (var f in found)
        // {
        //     Console.WriteLine($"[{f.Row},{f.Column}]");
        // }

        return found.Count.ToString();
    }

    private static List<Coordinate> SearchWord(char[][] grid, string word)
    {
        var result = new List<Coordinate>();

        var m = grid.Length;
        var n = grid[0].Length;

        for (var i = 0; i < m; i++)
        {
            for (var j = 0; j < n; j++)
            {
                var foundForCell = SearchAllDirections(grid, i, j, word);
                for (var k = 0; k < foundForCell; k++)
                {
                    result.Add(new Coordinate(i, j));
                }
            }
        }

        return result;
    }

    private static int SearchAllDirections(char[][] grid, int row, int col, string word)
    {
        var result = 0;

        var m = grid.Length;
        var n = grid[0].Length;

        var wordLength = word.Length;

        if(grid[row][col] != word[0])
            return result;

        var directions = new List<Coordinate>
        {
            new(-1, -1), new(-1, 0), new(-1, 1),
            new(0, -1),              new(0, 1),
            new(1, -1),  new(1, 0),  new(1, 1)
        };

        foreach (var dir in directions)
        {
            var currX = row + dir.Row;
            var currY = col + dir.Column;
            int i;

            for (i = 1; i < wordLength; i++)
            {
                if (currX >= m || currX < 0 || currY >= n || currY < 0)
                    break;

                if(grid[currX][currY] != word[i])
                    break;

                currX += dir.Row;
                currY += dir.Column;
            }

            if (i == wordLength)
                result++;
        }

        return result;
    }

    protected override string SolveTask2(string inputPath)
    {
        var grid = Helper.ReadToGrid(inputPath);

        var found = SearchShape(grid);

        // foreach (var f in found)
        // {
        //     Console.WriteLine($"[{f.Row},{f.Column}]");
        // }

        return found.Count.ToString();
    }

    private static List<Coordinate> SearchShape(char[][] grid)
    {
        var result = new List<Coordinate>();

        var m = grid.Length;
        var n = grid[0].Length;

        for (var i = 0; i < m; i++)
        {
            for (var j = 0; j < n; j++)
            {
                var foundForCell = SearchXDirections(grid, i, j);
                for (var k = 0; k < foundForCell; k++)
                {
                    result.Add(new Coordinate(i, j));
                }
            }
        }

        return result;
    }
    
    private static int SearchXDirections(char[][] grid, int row, int col)
    {
        var result = 0;

        var m = grid.Length;
        var n = grid[0].Length;

        if(grid[row][col] != 'A')
            return result;

        var directions = new Coordinate[][]
        {
            [new(-1, -1), new(1, 1)],
            [new(1, -1),  new(-1, 1)]
        };

        var found = 0;
        foreach (var dir in directions)
        {
            var currX = row + dir[0].Row;
            var currY = col + dir[0].Column;

            if (currX >= m || currX < 0 || currY >= n || currY < 0)
                break;

            if(grid[currX][currY] != 'M' && grid[currX][currY] != 'S')
                break;

            var leftover = grid[currX][currY] == 'M' ? 'S' : 'M';
            currX = row + dir[1].Row;
            currY = col + dir[1].Column;

            if (currX >= m || currX < 0 || currY >= n || currY < 0)
                break;

            if (grid[currX][currY] == leftover)
                found++;
        }

        if (found == 2)
            result++;

        return result;
    }
}