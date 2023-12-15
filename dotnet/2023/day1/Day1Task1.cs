internal sealed class Day1Task1 : DayBase
{
    public Day1Task1() : base(1)
    {
    }

    protected override string Solve(string inputPath)
    {
        var sum = 0;

        using var reader = new StreamReader(inputPath);
        var line = reader.ReadLine();
        while(line != null)
        {
            var firstDigit = GetFirstDigit(line);
            var lastDigit = GetLastDigit(line);
            sum+= firstDigit * 10 + lastDigit;
            line = reader.ReadLine();
        }
        return sum.ToString();
    }

    private static int GetFirstDigit(string line)
    {
        for(var i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]))
            {
                return int.Parse(line[i].ToString());
            }
        }
        throw new Exception("Can not find any digits in line");
    }
    private int GetLastDigit(string line)
    {
        for(var i = line.Length - 1; i >= 0; i--)
        {
            if (Char.IsDigit(line[i]))
            {
                return int.Parse(line[i].ToString());
            }
        }
        throw new Exception("Can not find any digits in line");
    }
}