namespace PlatformInfo.Linux;

using System;
using System.Globalization;

using PlatformInfo.Abstraction;

public sealed class UptimeInfo : IPlatformInfo
{
    public DateTime UpdateAt { get; private set; }

    public TimeSpan Uptime { get; private set; }

    public UptimeInfo()
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

        var span = File.ReadAllText("/proc/uptime").AsSpan();
        var range = (Span<Range>)stackalloc Range[2];
        span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries);
        var second = Double.Parse(span[range[0]], CultureInfo.InvariantCulture);
        Uptime = TimeSpan.FromSeconds(second);

        UpdateAt = now;

        return true;
    }
}
