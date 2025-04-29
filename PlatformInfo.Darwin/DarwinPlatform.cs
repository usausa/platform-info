namespace PlatformInfo.Darwin;

using System.Runtime.Versioning;

#pragma warning disable CA1024
[SupportedOSPlatform("maccatalyst")]
public static class DarwinPlatform
{
    public static UptimeInfo GetUptime() => new();
}
