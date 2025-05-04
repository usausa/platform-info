namespace PlatformInfo.Linux;

using System.Text.RegularExpressions;

using PlatformInfo.Abstraction;

public sealed class ProcessSummaryInfo : IPlatformInfo
{
    private readonly Regex regex = new(@"^\d+$", RegexOptions.Compiled);

    public DateTime UpdateAt { get; private set; }

    public int ProcessCount { get; private set; }

    public int ThreadCount { get; private set; }

    internal ProcessSummaryInfo()
    {
        Update();
    }

    public bool Update()
    {
        var process = 0;
        var thread = 0;
        foreach (var dir in Directory.EnumerateDirectories("/proc"))
        {
            if (!regex.IsMatch(Path.GetFileName(dir)))
            {
                continue;
            }

            process++;

            var statusFilePath = Path.Combine(dir, "status");
            if (!File.Exists(statusFilePath))
            {
                continue;
            }

            using var reader = new StreamReader(statusFilePath);
            while (reader.ReadLine() is { } line)
            {
                var span = line.AsSpan();

                if (span.StartsWith("Threads:"))
                {
                    thread += ExtractInt32(span);
                }
            }
        }

        ProcessCount = process;
        ThreadCount = thread;

        UpdateAt = DateTime.Now;

        return true;
    }

    private static int ExtractInt32(ReadOnlySpan<char> span)
    {
        var range = (Span<Range>)stackalloc Range[2];
        return (span.Split(range, '\t', StringSplitOptions.RemoveEmptyEntries) > 1) && Int32.TryParse(span[range[1]], out var result) ? result : 0;
    }
}
