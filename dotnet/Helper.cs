namespace dotnet;

internal sealed class Helper
{
    public static char[][] ReadToGrid(string inputPath)
    {
        using var reader = new StreamReader(inputPath);

        List<char[]> data = [];

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!.ToCharArray();
            data.Add(line);
        }

        return data.ToArray();
    }
}