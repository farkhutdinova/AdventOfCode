internal sealed class Day1Task1 : DayBase
{
    private readonly Dictionary<string, int> _digitWords3 = new()
    {
        {"one",1},
        {"two",2},
        {"six",6}
    };
    private readonly Dictionary<string, int> _digitWords4 = new()
    {
        {"four",4},
        {"five",5},
        {"nine",9}
    };
    private readonly Dictionary<string, int> _digitWords5 = new()
    {
        {"three",3},
        {"seven",7},
        {"eight",8}
    };

    public Day1Task1() : base(1)
    {
    }

    protected override string SolveTask1(string inputPath)
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

    protected override string SolveTask2(string inputPath)
    {
        var sum = 0;

        using var reader = new StreamReader(inputPath);
        var line = reader.ReadLine();
        while(line != null)
        {
            var firstDigit = GetFirstDigitWord(line);
            var lastDigit = GetLastDigitWord(line);
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

    private int GetFirstDigitWord(string line)
    {
        for(var i = 0; i < line.Length; i++)
        {
            if (Char.IsDigit(line[i]))
            {
                return int.Parse(line[i].ToString());
            }
            else
            {
                if (i + 2 < line.Length)
                {
                    var threeChar = line.Substring(i, 3);
                    if (_digitWords3.TryGetValue(threeChar, out var v3))
                    {
                        return v3;
                    }
                }
                if (i + 4 < line.Length)
                {
                    var fourChar = line.Substring(i, 4);
                    if (_digitWords4.TryGetValue(fourChar, out var v4))
                    {
                        return v4;
                    }
                }
                if (i + 5 < line.Length)
                {
                    var fiveChar = line.Substring(i, 5);
                    if (_digitWords5.TryGetValue(fiveChar, out var v5))
                    {
                        return v5;
                    }
                }
            }
        }
        throw new Exception("Can not find any digits in line");
    }

    private static int GetLastDigit(string line)
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

    private int GetLastDigitWord(string line)
    {
        for(var i = line.Length - 1; i >= 0; i--)
        {
            if (Char.IsDigit(line[i]))
            {
                return int.Parse(line[i].ToString());
            }
            else
            {
                if (i - 2 >= 0)
                {
                    var threeChar = line.Substring(i - 2, 3);
                    if (_digitWords3.TryGetValue(threeChar, out var v3))
                    {
                        return v3;
                    }
                }
                if (i - 3 >= 0)
                {
                    var fourChar = line.Substring(i - 3, 4);
                    if (_digitWords4.TryGetValue(fourChar, out var v4))
                    {
                        return v4;
                    }
                }
                if (i - 4 >= 0)
                {
                    var fiveChar = line.Substring(i - 4, 5);
                    if (_digitWords5.TryGetValue(fiveChar, out var v5))
                    {
                        return v5;
                    }
                }
            }
        }
        throw new Exception("Can not find any digits in line");
    }
}