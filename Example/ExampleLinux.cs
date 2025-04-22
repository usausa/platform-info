namespace Example;

using PlatformInfo.Linux;

#pragma warning disable CA1416
internal static class ExampleLinux
{
    public static void LinuxMain()
    {
        var uptime = LinuxPlatform.GetUptimeInfo();
        Console.WriteLine(uptime.Uptime);
        //Console.WriteLine(LinuxPlatform.MemoryTest());
        //LinuxPlatform.CpuTest();
    }
}
