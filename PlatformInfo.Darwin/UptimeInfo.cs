namespace PlatformInfo.Darwin;

using System.Globalization;
using System.Runtime.InteropServices;

using static PlatformInfo.Darwin.NativeMethods;

public sealed class UptimeInfo
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

        var time = new timeval { tv_sec = 0, tv_usec = 0 };
        var size = Marshal.SizeOf<timeval>();
        if (sysctlbyname("kern.boottime", ref time, ref size, IntPtr.Zero, 0) != 0)
        {
            return false;
        }

        var boot = DateTimeOffset.FromUnixTimeMilliseconds((time.tv_sec * 1000) + (time.tv_usec / 1000));
        Uptime = DateTimeOffset.Now - boot;

        UpdateAt = now;

        return true;
    }
}
