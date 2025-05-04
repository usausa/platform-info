namespace PlatformInfo.Linux;

public sealed class CpuCore
{
    public string Name { get; }

    internal CpuCore(string name)
    {
        Name = name;
    }
}

public sealed class CpuDevice
{
    internal CpuDevice()
    {
        // TODO CPU frequency
        // TODO CPU power ? (Intel)

        // TODO
    }
}
