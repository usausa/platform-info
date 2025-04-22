namespace PlatformInfo.Linux;

using System.Globalization;
using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
public static class LinuxPlatform
{
    public static TimeSpan GetUptime()
    {
        var str = File.ReadAllText("/proc/uptime");
        var second = Double.Parse(str.Split(' ')[0], CultureInfo.InvariantCulture);
        return TimeSpan.FromSeconds(second);
    }
}
