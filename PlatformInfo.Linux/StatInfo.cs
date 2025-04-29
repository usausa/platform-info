namespace PlatformInfo.Linux;

using System;
using System.Globalization;

using PlatformInfo.Abstraction;

public sealed class CpuStat
{
    private long previousUser;

    private long previousNice;

    private long previousSystem;

    private long previousIdle;

    private long previousIoWait;

    private long previousIrq;

    private long previousSoftIrq;

    private long previousSteal;

    private long previousGuest;

    private long previousGuestNice;

    public long UserTotal { get; internal set; }

    public long NiceTotal { get; internal set; }

    public long SystemTotal { get; internal set; }

    public long IdleTotal { get; internal set; }

    public long IoWaitTotal { get; internal set; }

    public long IrqTotal { get; internal set; }

    public long SoftIrqTotal { get; internal set; }

    public long StealTotal { get; internal set; }

    public long GuestTotal { get; internal set; }

    public long GuestNiceTotal { get; internal set; }

    public long UserDiff => UserTotal - previousUser;

    public long NiceDiff => NiceTotal - previousNice;

    public long SystemDiff => SystemTotal - previousSystem;

    public long IdleDiff => IdleTotal - previousIdle;

    public long IoWaitDiff => IoWaitTotal - previousIoWait;

    public long IrqDiff => IrqTotal - previousIrq;

    public long SoftIrqDiff => SoftIrqTotal - previousSoftIrq;

    public long StealDiff => StealTotal - previousSteal;

    public long GuestDiff => GuestTotal - previousGuest;

    public long GuestNiceDiff => GuestNiceTotal - previousGuestNice;

    internal long TimeSpanActive => UserDiff + NiceDiff + SystemDiff + IoWaitDiff + IrqDiff + SoftIrqDiff;

    internal long TimeSpanTotal => TimeSpanActive + IdleDiff;

    public double Usage
    {
        get
        {
            var total = TimeSpanTotal;
            var active = TimeSpanActive;
            return total == 0 ? 0 : (double)active / total * 100;
        }
    }

    internal void CopyToPrevious()
    {
        previousUser = UserTotal;
        previousNice = NiceTotal;
        previousSystem = SystemTotal;
        previousIdle = IdleTotal;
        previousIoWait = IoWaitTotal;
        previousIrq = IrqTotal;
        previousSoftIrq = SoftIrqTotal;
        previousSteal = StealTotal;
        previousGuest = GuestTotal;
        previousGuestNice = GuestNiceTotal;
    }
}

public sealed class StatInfo : ITimeSpanPlatformInfo
{
    private readonly List<CpuStat> cpu = new();

    private DateTime previousUpdateAt;

    private long previousInterrupt;

    private long previousContextSwitch;

    private long previousSoftIrq;

    public DateTime UpdateAt { get; private set; }

    public TimeSpan TimeSpan => UpdateAt - previousUpdateAt;

    public CpuStat CpuTotal { get; } = new();

    public IReadOnlyList<CpuStat> Cpu => cpu;

    public long InterruptTotal { get; private set; }

    public long InterruptDiff => InterruptTotal - previousInterrupt;

    public double InterruptPerSecond => InterruptDiff / TimeSpan.TotalSeconds;

    public long ContextSwitchTotal { get; private set; }

    public long ContextSwitchDiff => ContextSwitchTotal - previousContextSwitch;

    public double ContextSwitchPerSecond => ContextSwitchDiff / TimeSpan.TotalSeconds;

    public long SoftIrqTotal { get; private set; }

    public long SoftIrqDiff => SoftIrqTotal - previousSoftIrq;

    public double SoftIrqPerSecond => SoftIrqDiff / TimeSpan.TotalSeconds;

    public int ProcessRunning { get; private set; }

    public int ProcessBlocked { get; private set; }

    public StatInfo()
    {
        Update();

        CpuTotal.CopyToPrevious();
        foreach (var stat in cpu)
        {
            stat.CopyToPrevious();
        }

        previousInterrupt = InterruptTotal;
        previousContextSwitch = ContextSwitchTotal;
        previousSoftIrq = SoftIrqTotal;
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
            else if (span.StartsWith("intr"))
            {
                previousInterrupt = InterruptTotal;
                InterruptTotal = ExtractInt64(span, 3);
            }
            else if (span.StartsWith("ctxt"))
            {
                previousContextSwitch = ContextSwitchTotal;
                ContextSwitchTotal = ExtractInt64(span);
            }
            else if (span.StartsWith("procs_running"))
            {
                ProcessRunning = ExtractInt32(span);
            }
            else if (span.StartsWith("procs_blocked"))
            {
                ProcessBlocked = ExtractInt32(span);
            }
            else if (span.StartsWith("softirq"))
            {
                previousSoftIrq = SoftIrqTotal;
                SoftIrqTotal = ExtractInt64(span, 12);
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

        stat.CopyToPrevious();
        stat.UserTotal = Int64.TryParse(span[range[1]], out var value) ? value : 0;
        stat.NiceTotal = Int64.TryParse(span[range[2]], out value) ? value : 0;
        stat.SystemTotal = Int64.TryParse(span[range[3]], out value) ? value : 0;
        stat.IdleTotal = Int64.TryParse(span[range[4]], out value) ? value : 0;
        stat.IoWaitTotal = Int64.TryParse(span[range[5]], out value) ? value : 0;
        stat.IrqTotal = Int64.TryParse(span[range[6]], out value) ? value : 0;
        stat.SoftIrqTotal = Int64.TryParse(span[range[7]], out value) ? value : 0;
        stat.StealTotal = Int64.TryParse(span[range[8]], out value) ? value : 0;
        stat.GuestTotal = Int64.TryParse(span[range[9]], out value) ? value : 0;
        stat.GuestNiceTotal = Int64.TryParse(span[range[10]], out value) ? value : 0;
    }

    private CpuStat FindCpu(int index)
    {
        while (index >= cpu.Count)
        {
            cpu.Add(new CpuStat());
        }

        return cpu[index];
    }

    private static long ExtractInt64(ReadOnlySpan<char> span, int n = 2)
    {
        var range = (Span<Range>)stackalloc Range[n];
        return (span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries) > 1) && Int64.TryParse(span[range[1]], out var result) ? result : 0;
    }

    private static int ExtractInt32(ReadOnlySpan<char> span, int n = 2)
    {
        var range = (Span<Range>)stackalloc Range[n];
        return (span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries) > 1) && Int32.TryParse(span[range[1]], out var result) ? result : 0;
    }
}
