using System.Text.RegularExpressions;

internal sealed class Day02 : DayBase
{
    private readonly int _redMax = 12;
    private readonly int _greenMax = 13;
    private readonly int _blueMax = 14;

    public Day02() : base(2)
    {
    }

    protected override string SolveTask1(string inputPath)
    {
        var sum = 0;

        using var reader = new StreamReader(inputPath);
        var line = reader.ReadLine();
        while(line != null)
        {
            var possibleId = CheckGame(line);
            if (possibleId.HasValue)
                sum+= possibleId.Value;
            line = reader.ReadLine();
        }
        return sum.ToString();
    }

    protected override string SolveTask2(string inputPath)
    {
        throw new NotImplementedException();
    }

    private int? CheckGame(string line)
    {
        var columnIndex = line.IndexOf(':');
        var gameId = int.Parse(line.Substring(5, columnIndex - 5));
        var cubes = line.Substring(columnIndex + 1).Split(',', ';');
        var isGameValid = true;
        foreach(var c in cubes)
        {
            var cInfo = c.Trim().Split(' ');
            var count = int.Parse(cInfo[0]);
            var color = cInfo[1];
            var isValid = color switch
            {
                "red" => count <= _redMax,
                "green" => count <= _greenMax,
                "blue" => count <= _blueMax
            };
            if (isValid == false)
            {
                isGameValid = false;
                break;
            }
        }
        return isGameValid ? gameId : null;
    }
}