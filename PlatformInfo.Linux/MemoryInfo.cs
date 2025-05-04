namespace PlatformInfo.Linux;

using System;

using PlatformInfo.Abstraction;

public sealed class MemoryInfo : IPlatformInfo
{
    public DateTime UpdateAt { get; private set; }

    public long MemoryTotal { get; private set; }

    public long MemFree { get; private set; }

    public long Buffers { get; private set; }

    public long Cached { get; private set; }

    public double MemoryUsage => MemoryTotal == 0 ? 0 : (double)(MemoryTotal - MemFree - Buffers - Cached) / MemoryTotal * 100;

    internal MemoryInfo()
    {
        Update();
    }

    public bool Update()
    {
        using var reader = new StreamReader("/proc/meminfo");
        while (reader.ReadLine() is { } line)
        {
            var span = line.AsSpan();
            if (span.StartsWith("MemTotal"))
            {
                MemoryTotal = ExtractInt64(span);
            }
            else if (span.StartsWith("MemFree"))
            {
                MemFree = ExtractInt64(span);
            }
            else if (span.StartsWith("Buffers"))
            {
                Buffers = ExtractInt64(span);
            }
            else if (span.StartsWith("Cached"))
            {
                Cached = ExtractInt64(span);
            }
        }

        UpdateAt = DateTime.Now;

        return true;
    }

    private static long ExtractInt64(ReadOnlySpan<char> span)
    {
        var range = (Span<Range>)stackalloc Range[3];
        return (span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries) > 1) && Int64.TryParse(span[range[1]], out var result) ? result : 0;
    }
}
