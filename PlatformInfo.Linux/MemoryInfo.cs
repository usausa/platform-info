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

    public MemoryInfo()
    {
        Update();
    }

    public bool Update()
    {
        var now = DateTime.Now;
        if (UpdateAt == now)
        {
            return true;
        }

        using var reader = new StreamReader("/proc/meminfo");
        while (reader.ReadLine() is { } line)
        {
            var span = line.AsSpan();
            if (span.StartsWith("MemTotal"))
            {
                MemoryTotal = ExtractValue(span);
            }
            else if (span.StartsWith("MemFree"))
            {
                MemFree = ExtractValue(span);
            }
            else if (span.StartsWith("Buffers"))
            {
                Buffers = ExtractValue(span);
            }
            else if (span.StartsWith("Cached"))
            {
                Cached = ExtractValue(span);
            }
        }

        UpdateAt = now;

        return true;
    }

    private static long ExtractValue(ReadOnlySpan<char> span)
    {
        var range = (Span<Range>)stackalloc Range[3];
        return (span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries) > 1) && Int64.TryParse(span[range[1]], out var result) ? result : 0;
    }
}
