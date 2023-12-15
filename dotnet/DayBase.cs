internal abstract class DayBase
{
    private readonly string _year = "2023";

    private readonly string _task1Path;
    private readonly string _task1PathTest;

    public DayBase(int dayNumber)
    {
        string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName;
        _task1Path = Path.Combine(projectDirectory, _year, "day" + dayNumber, "input.txt");
        _task1PathTest = Path.Combine(projectDirectory, _year, "day" + dayNumber, "test1.txt");
    }

    public string GetInputTask1()
    {
        // return Solve(_task1PathTest);
        return Solve(_task1Path);
    }

    protected abstract string Solve(string inputPath);
}