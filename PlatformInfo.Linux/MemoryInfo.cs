namespace PlatformInfo.Linux;

using System;

public sealed class MemoryInfo
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

        var count = 0;
        var memoryTotal = 0L;
        var memoryFree = 0L;
        var buffers = 0L;
        var cached = 0L;

        using var reader = new StreamReader("/proc/meminfo");
        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith("MemTotal", StringComparison.Ordinal))
            {
                memoryTotal = ExtractValue(line.AsSpan());
                count++;
            }
            else if (line.StartsWith("MemFree", StringComparison.Ordinal))
            {
                memoryFree = ExtractValue(line.AsSpan());
                count++;
            }
            else if (line.StartsWith("Buffers", StringComparison.Ordinal))
            {
                buffers = ExtractValue(line.AsSpan());
                count++;
            }
            else if (line.StartsWith("Cached", StringComparison.Ordinal))
            {
                cached = ExtractValue(line.AsSpan());
                count++;
            }
        }

        if (count < 4)
        {
            return false;
        }

        MemoryTotal = memoryTotal;
        MemFree = memoryFree;
        Buffers = buffers;
        Cached = cached;

        UpdateAt = now;

        return true;

        long ExtractValue(ReadOnlySpan<char> span)
        {
            var range = (Span<Range>)stackalloc Range[3];
            return (span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries) > 1) && Int64.TryParse(span[range[1]], out var result) ? result : 0;
        }
    }
}
