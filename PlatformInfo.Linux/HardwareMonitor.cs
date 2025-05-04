namespace PlatformInfo.Linux;

public sealed class HardwareSensor
{
    internal HardwareSensor()
    {
    }
}

public sealed class HardwareMonitor
{
    internal HardwareMonitor()
    {
    }

    internal static HardwareMonitor[] GetMonitors()
    {
        return [];
    }
}
