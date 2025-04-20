namespace Example;

#pragma warning disable CA1416
internal static class ExampleDarwin
{
    public static void DarwinMain()
    {
        Console.WriteLine(PlatformInfo.Darwin.Platform.GetUptime());
    }
}
