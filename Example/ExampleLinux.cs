namespace Example;

using System.Globalization;
using System.Text;

using PlatformInfo.Linux;

#pragma warning disable CA1416
internal static class ExampleLinux
{
    public static void LinuxMain()
    {
        var uptime = LinuxPlatform.GetUptimeInfo();
        Console.WriteLine(uptime.Uptime);

        var load = LinuxPlatform.GetLoadAverageInfo();

        var stat = LinuxPlatform.GetStatInfo();
        Thread.Sleep(1000);
        stat.Update();

        Console.WriteLine(stat.CpuTotal.Usage);
        Console.WriteLine(stat.ProcessRunning);
        Console.WriteLine(stat.ProcessBlocked);
        Console.WriteLine(stat.ContextSwitchPerSecond);

        Console.WriteLine("Total    Cpu1   Cpu2   Cpu3   Cpu4");
        for (var i = 0; i < 100; i++)
        {
            Thread.Sleep(1000);
            stat.Update();
            load.Update();

            var sb = new StringBuilder();
            sb.Append(stat.CpuTotal.Usage.ToString("F2", CultureInfo.InvariantCulture).PadLeft(7));
            foreach (var cpu in stat.Cpu)
            {
                sb.Append(cpu.Usage.ToString("F2", CultureInfo.InvariantCulture).PadLeft(7));
            }

            Console.WriteLine(sb);
            Console.WriteLine($"{load.Average1:F2} {load.Average5:F2} {load.Average15:F2}");
        }

        var memory = LinuxPlatform.GetMemoryInfo();
        Console.WriteLine(memory.MemoryUsage);
        //LinuxPlatform.CpuTest();

        var drives = DriveInfo.GetDrives();
        foreach (var drive in drives)
        {
            if (drive.IsReady && IsTargetDriveType(drive.DriveType))
            {
                var totalSize = drive.TotalSize;
                var freeSpace = drive.TotalFreeSpace;
                var usedSpace = totalSize - freeSpace;

                var usagePercentage = ((double)usedSpace / totalSize) * 100;

                Console.WriteLine($"Drive: {drive.Name} {drive.VolumeLabel}");
                Console.WriteLine($"  Type: {drive.DriveType}");
                Console.WriteLine($"  Format: {drive.DriveFormat}");
                Console.WriteLine($"  Total Size: {FormatBytes(totalSize)}");
                Console.WriteLine($"  Used Space: {FormatBytes(usedSpace)}");
                Console.WriteLine($"  Usage: {usagePercentage:F2}%");
            }
        }
    }

    private static bool IsTargetDriveType(DriveType type) =>
        type != DriveType.Removable &&
        type != DriveType.Network &&
        type != DriveType.CDRom &&
        type != DriveType.Ram;

    private static string FormatBytes(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        var order = 0;
        double len = bytes;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:F2} {sizes[order]}";
    }
}
