namespace Example;

using PlatformInfo.Darwin;

#pragma warning disable CA1416
internal static class ExampleDarwin
{
    public static void DarwinMain()
    {
        var uptime = DarwinPlatform.GetUptimeInfo();
        Console.WriteLine(uptime.Uptime);
    }
}
