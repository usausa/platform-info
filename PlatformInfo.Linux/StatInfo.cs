namespace PlatformInfo.Linux;

using System;
using System.Globalization;

using PlatformInfo.Abstraction;

public sealed class CpuStat
{
    public long User { get; internal set; }

    public long Nice { get; internal set; }

    public long System { get; internal set; }

    public long Idle { get; internal set; }

    public long IoWait { get; internal set; }

    public long Irq { get; internal set; }

    public long SoftIrq { get; internal set; }

    public long Steal { get; internal set; }

    public long Guest { get; internal set; }

    public long GuestNice { get; internal set; }

    internal long PreviousActive { get; set; }

    internal long PreviousTotal { get; set; }

    internal long Active => User + Nice + System + IoWait + Irq + SoftIrq;

    internal long Total => Active + Idle;

    public double Usage
    {
        get
        {
            var totalDiff = Total - PreviousTotal;
            if (totalDiff == 0)
            {
                return 0;
            }

            var activeDiff = Active - PreviousActive;
            return (double)activeDiff / totalDiff * 100;
        }
    }

    internal void Initialize()
    {
        PreviousActive = Active;
        PreviousTotal = Total;
    }
}

public sealed class StatInfo : IPlatformInfo
{
    private readonly List<CpuStat> cpu = new();

    private DateTime previousUpdateAt;

    private long previousContextSwitch;

    public DateTime UpdateAt { get; private set; }

    public CpuStat CpuTotal { get; } = new();

    public IReadOnlyList<CpuStat> Cpu => cpu;

    public long ContextSwitchTotal { get; private set; }

    public double ContextSwitchPerSecond => (ContextSwitchTotal - previousContextSwitch) / (UpdateAt - previousUpdateAt).TotalSeconds;

    public long ProcessRunning { get; private set; }

    public long ProcessBlocked { get; private set; }

    public StatInfo()
    {
        Update();

        CpuTotal.Initialize();
        foreach (var stat in cpu)
        {
            stat.Initialize();
        }

        previousContextSwitch = ContextSwitchTotal;
        previousUpdateAt = UpdateAt;
    }

    public bool Update()
    {
        var now = DateTime.Now;
        if (UpdateAt == now)
        {
            return true;
        }

        using var reader = new StreamReader("/proc/stat");
        while (reader.ReadLine() is { } line)
        {
            var span = line.AsSpan();

            if (span.StartsWith("cpu"))
            {
                UpdateCpuValue(span);
            }
            else if (span.StartsWith("ctxt"))
            {
                previousContextSwitch = ContextSwitchTotal;
                ContextSwitchTotal = ExtractValue(span);
            }
            else if (span.StartsWith("procs_running"))
            {
                ProcessRunning = ExtractValue(span);
            }
            else if (span.StartsWith("procs_blocked"))
            {
                ProcessBlocked = ExtractValue(span);
            }
        }

        previousUpdateAt = UpdateAt;
        UpdateAt = now;

        return true;
    }

    private void UpdateCpuValue(ReadOnlySpan<char> span)
    {
        var range = (Span<Range>)stackalloc Range[11];
        span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries);

        CpuStat stat;
        if (span[range[0]] is "cpu")
        {
            stat = CpuTotal;
        }
        else
        {
            var index = Int32.Parse(span[range[0]][3..], CultureInfo.InvariantCulture);
            stat = FindCpu(index);
        }

        stat.PreviousActive = stat.Active;
        stat.PreviousTotal = stat.Total;

        stat.User = Int64.TryParse(span[range[1]], out var value) ? value : 0;
        stat.Nice = Int64.TryParse(span[range[2]], out value) ? value : 0;
        stat.System = Int64.TryParse(span[range[3]], out value) ? value : 0;
        stat.Idle = Int64.TryParse(span[range[4]], out value) ? value : 0;
        stat.IoWait = Int64.TryParse(span[range[5]], out value) ? value : 0;
        stat.Irq = Int64.TryParse(span[range[6]], out value) ? value : 0;
        stat.SoftIrq = Int64.TryParse(span[range[7]], out value) ? value : 0;
        stat.Steal = Int64.TryParse(span[range[8]], out value) ? value : 0;
        stat.Guest = Int64.TryParse(span[range[9]], out value) ? value : 0;
        stat.GuestNice = Int64.TryParse(span[range[10]], out value) ? value : 0;
    }

    private CpuStat FindCpu(int index)
    {
        while (index >= cpu.Count)
        {
            cpu.Add(new CpuStat());
        }

        return cpu[index];
    }

    private static long ExtractValue(ReadOnlySpan<char> span)
    {
        var range = (Span<Range>)stackalloc Range[2];
        return (span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries) > 1) && Int64.TryParse(span[range[1]], out var result) ? result : 0;
    }
}
