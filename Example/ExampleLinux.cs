namespace Example;

using PlatformInfo.Linux;

#pragma warning disable CA1416
internal static class ExampleLinux
{
    public static void LinuxMain()
    {
        var uptime = LinuxPlatform.GetUptimeInfo();
        Console.WriteLine(uptime.Uptime);

        var memory = LinuxPlatform.GetMemoryInfo();
        Console.WriteLine(memory.MemoryUsage);
        //LinuxPlatform.CpuTest();

        var drives = DriveInfo.GetDrives();
        foreach (var drive in drives)
        {
            if (drive.IsReady && drive.DriveType == DriveType.Fixed)
            {
                var driveName = drive.Name;
                var totalSize = drive.TotalSize;
                var freeSpace = drive.TotalFreeSpace;
                var usedSpace = totalSize - freeSpace;

                // 使用率を計算
                var usagePercentage = ((double)usedSpace / totalSize) * 100;

                // 結果を表示
                Console.WriteLine($"Drive: {driveName}");
                //Console.WriteLine($"  Total Size: {FormatBytes(totalSize)}");
                //Console.WriteLine($"  Used Space: {FormatBytes(usedSpace)}");
                //Console.WriteLine($"  Free Space: {FormatBytes(freeSpace)}");
                Console.WriteLine($"  Usage: {usagePercentage:F2}%");
            }
        }
    }

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
