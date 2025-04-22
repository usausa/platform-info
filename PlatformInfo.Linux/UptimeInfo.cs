namespace PlatformInfo.Linux;

using System.Globalization;

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

        var str = File.ReadAllText("/proc/uptime");
        var second = Double.Parse(str.Split(' ')[0], CultureInfo.InvariantCulture);
        Uptime = TimeSpan.FromSeconds(second);

        UpdateAt = now;

        return true;
    }
}
