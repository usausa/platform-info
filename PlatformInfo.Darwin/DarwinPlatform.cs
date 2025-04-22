namespace PlatformInfo.Darwin;

using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using static PlatformInfo.Darwin.NativeMethods;

[SupportedOSPlatform("maccatalyst")]
public static class DarwinPlatform
{
    public static TimeSpan GetUptime()
    {
        var time = new timeval { tv_sec = 0, tv_usec = 0 };
        var size = Marshal.SizeOf<timeval>();
        if (sysctlbyname("kern.boottime", ref time, ref size, IntPtr.Zero, 0) != 0)
        {
            return TimeSpan.Zero;
        }

        var boot = DateTimeOffset.FromUnixTimeMilliseconds((time.tv_sec * 1000) + (time.tv_usec / 1000));
        return DateTimeOffset.Now - boot;
    }
}
