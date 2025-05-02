namespace Example;

using System.Globalization;
using System.Text;

using PlatformInfo.Linux;

#pragma warning disable CA1416
internal static class ExampleLinux
{
    public static void LinuxMain()
    {
        var uptime = LinuxPlatform.GetUptime();
        Console.WriteLine(uptime.Uptime);

        var fd = LinuxPlatform.GetFileDescriptor();
        Console.WriteLine($"{fd.Allocated} {fd.Used} {fd.Max}");

        var process = LinuxPlatform.GetProcessSummary();
        Console.WriteLine($"{process.ProcessCount} {process.ThreadCount}");

        var load = LinuxPlatform.GetLoadAverage();

        var stat = LinuxPlatform.GetStat();
        Thread.Sleep(1000);
        stat.Update();

        Console.WriteLine(stat.CpuTotal.Usage);
        Console.WriteLine(stat.ProcessRunning);
        Console.WriteLine(stat.ProcessBlocked);
        Console.WriteLine(stat.ContextSwitchPerSecond);

        for (var i = 0; i < 10; i++)
        {
            Thread.Sleep(1000);
            stat.Update();

            Console.WriteLine($"{stat.ContextSwitchTotal} {stat.InterruptTotal} {stat.SoftIrqTotal}");
            Console.WriteLine($"{stat.ContextSwitchPerSecond:F2} {stat.InterruptPerSecond:F2} {stat.SoftIrqPerSecond:F2}");
        }

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

        var memory = LinuxPlatform.GetMemory();
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
