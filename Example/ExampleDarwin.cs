namespace Example;

using PlatformInfo.Darwin;

#pragma warning disable CA1416
internal static class ExampleDarwin
{
    public static void DarwinMain()
    {
        var uptime = DarwinPlatform.GetUptimeInfo();
        Console.WriteLine(uptime.Uptime);

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
