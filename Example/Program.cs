namespace Example;

using System.Runtime.InteropServices;

internal static class Program
{
    public static void Main()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            ExampleLinux.LinuxMain();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            ExampleDarwin.DarwinMain();
        }
    }
}
