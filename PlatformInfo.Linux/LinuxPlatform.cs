namespace PlatformInfo.Linux;

using System;
using System.Globalization;
using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
public static unsafe class LinuxPlatform
{
    // TODO Update, Values方式？

    public static TimeSpan GetUptime()
    {
        var str = File.ReadAllText("/proc/uptime");
        var second = Double.Parse(str.Split(' ')[0], CultureInfo.InvariantCulture);
        return TimeSpan.FromSeconds(second);
    }

    public static void CpuTest()
    {
        // TODO
    }

    public static double MemoryTest()
    {
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
            }
            else if (line.StartsWith("MemFree", StringComparison.Ordinal))
            {
                memoryFree = ExtractValue(line.AsSpan());
            }
            else if (line.StartsWith("Buffers", StringComparison.Ordinal))
            {
                buffers = ExtractValue(line.AsSpan());
            }
            else if (line.StartsWith("Cached", StringComparison.Ordinal))
            {
                cached = ExtractValue(line.AsSpan());
            }
        }

        if (memoryTotal == 0)
        {
            return 0;
        }

        var used = memoryTotal - memoryFree - buffers - cached;
        return (double)used / memoryTotal * 100;

        long ExtractValue(ReadOnlySpan<char> span)
        {
            var range = (Span<Range>)stackalloc Range[3];
            return (span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries) > 1) && Int64.TryParse(span[range[1]], out var result) ? result : 0;
        }
    }
}
