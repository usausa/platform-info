namespace PlatformInfo.Linux;

using System.Runtime.Versioning;

#pragma warning disable CA1024
[SupportedOSPlatform("linux")]
public static class LinuxPlatform
{
    public static UptimeInfo GetUptime() => new();

    public static StatInfo GetStat() => new();

    public static LoadAverageInfo GetLoadAverage() => new();

    public static MemoryInfo GetMemory() => new();

    public static VirtualMemoryInfo GetVirtualMemory() => new();

    public static FileDescriptorInfo GetFileDescriptor() => new();

    public static ProcessSummaryInfo GetProcessSummary() => new();
}
