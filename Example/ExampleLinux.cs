namespace Example;

using PlatformInfo.Linux;

#pragma warning disable CA1416
internal static class ExampleLinux
{
    public static void LinuxMain()
    {
        Console.WriteLine(LinuxPlatform.GetUptime());
    }
}
