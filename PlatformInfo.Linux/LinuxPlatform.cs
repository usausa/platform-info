namespace PlatformInfo.Linux;

using System.Runtime.Versioning;

#pragma warning disable CA1024
[SupportedOSPlatform("linux")]
public static class LinuxPlatform
{
    public static UptimeInfo GetUptime() => new();

    public static StaticsInfo GetStatics() => new();

    public static LoadAverageInfo GetLoadAverage() => new();

    public static MemoryInfo GetMemory() => new();

    public static VirtualMemoryInfo GetVirtualMemory() => new();

    public static Partition[] GetPartitions() => Partition.GetPartitions();

    public static DiskStaticsInfo GetDiskStatics() => new();

    public static FileDescriptorInfo GetFileDescriptor() => new();

    public static NetworkStaticInfo GetNetworkStatic() => new();

    public static TcpInfo GetTcp() => new();

    public static TcpInfo GetTcp6() => new(6);

    public static ProcessSummaryInfo GetProcessSummary() => new();

    public static BatteryInfo GetBatteryInfo() => new();

    public static AdapterInfo GetAdapterInfo() => new();

    // TODO Temperature sensor

    // TODO CPU frequency

    // TODO CPU power ? (Intel)

    // TODO SMART ? (feature)

    // TODO Docker (feature)
}
