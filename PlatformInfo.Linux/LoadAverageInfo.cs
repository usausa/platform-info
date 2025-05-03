namespace PlatformInfo.Linux;

using System;
using System.Globalization;

using PlatformInfo.Abstraction;

public sealed class LoadAverageInfo : IPlatformInfo
{
    public DateTime UpdateAt { get; private set; }

    public double Average1 { get; private set; }

    public double Average5 { get; private set; }

    public double Average15 { get; private set; }

    internal LoadAverageInfo()
    {
        Update();
    }

    // ReSharper disable StringLiteralTypo
    public bool Update()
    {
        var now = DateTime.Now;
        if (UpdateAt == now)
        {
            return true;
        }

        var span = File.ReadAllText("/proc/loadavg").AsSpan();
        var range = (Span<Range>)stackalloc Range[5];
        span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries);
        Average1 = Double.Parse(span[range[0]], CultureInfo.InvariantCulture);
        Average5 = Double.Parse(span[range[1]], CultureInfo.InvariantCulture);
        Average15 = Double.Parse(span[range[2]], CultureInfo.InvariantCulture);

        UpdateAt = now;

        return true;
    }
    // ReSharper restore StringLiteralTypo
}
