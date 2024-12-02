namespace dotnet;

public abstract class DayBase
{
    private readonly string _year = "2024";

    private readonly string _task1Path;
    private readonly string _task1PathTest;
    private readonly string _task2Path;
    private readonly string _task2PathTest;

    protected DayBase(int dayNumber)
    {
        Number = dayNumber;
        var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName;
        _task1Path = Path.Combine(projectDirectory, _year, "day" + dayNumber, "input1.txt");
        _task1PathTest = Path.Combine(projectDirectory, _year, "day" + dayNumber, "test1.txt");
        _task2Path = Path.Combine(projectDirectory, _year, "day" + dayNumber, "input2.txt");
        _task2PathTest = Path.Combine(projectDirectory, _year, "day" + dayNumber, "test2.txt");
    }

    public int Number { get; }

    public string GetTestTask1()
    {
        return SolveTask1(_task1PathTest);
    }

    public string GetTestTask2()
    {
        return SolveTask2(_task2PathTest);
    }

    public string GetInputTask1()
    {
        return SolveTask1(_task1Path);
    }

    public string GetInputTask2()
    {
        return SolveTask2(_task2Path);
    }

    protected abstract string SolveTask1(string inputPath);

    protected abstract string SolveTask2(string inputPath);
}