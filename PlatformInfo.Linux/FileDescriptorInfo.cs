namespace PlatformInfo.Linux;

using System;
using System.Globalization;

public sealed class FileDescriptorInfo
{
    public DateTime UpdateAt { get; private set; }

    public long Allocated { get; private set; }

    public long Used { get; private set; }

    public long Max { get; private set; }

    internal FileDescriptorInfo()
    {
        Update();
    }

    public bool Update()
    {
        var span = File.ReadAllText("/proc/sys/fs/file-nr").AsSpan();
        var range = (Span<Range>)stackalloc Range[4];
        span.Split(range, '\t', StringSplitOptions.RemoveEmptyEntries);
        Allocated = Int64.Parse(span[range[0]], CultureInfo.InvariantCulture);
        Used = Int64.Parse(span[range[1]], CultureInfo.InvariantCulture);
        Max = Int64.Parse(span[range[2]], CultureInfo.InvariantCulture);

        UpdateAt = DateTime.Now;

        return true;
    }
}
