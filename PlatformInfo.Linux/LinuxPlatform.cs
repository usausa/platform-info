namespace PlatformInfo.Linux;

using System.Runtime.Versioning;

#pragma warning disable CA1024
[SupportedOSPlatform("linux")]
public static class LinuxPlatform
{
    public static UptimeInfo GetUptimeInfo() => new();

    public static StatInfo GetStatInfo() => new();

    public static MemoryInfo GetMemoryInfo() => new();
}
