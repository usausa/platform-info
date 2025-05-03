namespace PlatformInfo.Linux;

using System;
using System.Globalization;

using PlatformInfo.Abstraction;

public sealed class CpuStatics
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

    public long Active => User + Nice + System + IoWait + Irq + SoftIrq;

    public long Total => Active + Idle;

    internal CpuStatics()
    {
    }
}

public sealed class StaticsInfo : IPlatformInfo
{
    private readonly List<CpuStatics> cpu = new();

    public DateTime UpdateAt { get; private set; }

    public CpuStatics CpuTotal { get; } = new();

    public IReadOnlyList<CpuStatics> Cpu => cpu;

    // Total
    public long Interrupt { get; private set; }

    // Total
    public long ContextSwitch { get; private set; }

    // Total
    public long SoftIrq { get; private set; }

    public int ProcessRunning { get; private set; }

    public int ProcessBlocked { get; private set; }

    internal StaticsInfo()
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
                Interrupt = ExtractInt64(span, 3);
            }
            else if (span.StartsWith("ctxt"))
            {
                ContextSwitch = ExtractInt64(span);
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
                SoftIrq = ExtractInt64(span, 12);
            }
        }

        UpdateAt = now;

        return true;
    }
    // ReSharper restore StringLiteralTypo

    private void UpdateCpuValue(ReadOnlySpan<char> span)
    {
        var range = (Span<Range>)stackalloc Range[11];
        span.Split(range, ' ', StringSplitOptions.RemoveEmptyEntries);

        CpuStatics stat;
        if (span[range[0]] is "cpu")
        {
            stat = CpuTotal;
        }
        else
        {
            var index = Int32.Parse(span[range[0]][3..], CultureInfo.InvariantCulture);
            stat = FindCpu(index);
        }

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

    private CpuStatics FindCpu(int index)
    {
        while (index >= cpu.Count)
        {
            cpu.Add(new CpuStatics());
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
