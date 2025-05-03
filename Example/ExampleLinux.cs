namespace Example;

using System;
using System.Globalization;
using System.Text;

using PlatformInfo.Linux;

#pragma warning disable CA1416
internal static class ExampleLinux
{
    private sealed class CpuSnapshot
    {
        public long Active { get; private set; }

        public long Total { get; private set; }

        public void SnapshotFrom(CpuStat cpu)
        {
            Active = cpu.Active;
            Total = cpu.Total;
        }
    }

    private sealed class StatSnapshot
    {
        public DateTime UpdateAt { get; private set; }

        public long Interrupt { get; private set; }

        public long ContextSwitch { get; private set; }

        public long SoftIrq { get; private set; }

        public void SnapshotFrom(StatInfo stat)
        {
            UpdateAt = stat.UpdateAt;
            Interrupt = stat.Interrupt;
            ContextSwitch = stat.ContextSwitch;
            SoftIrq = stat.SoftIrq;
        }
    }

    private static double CalcUsage(CpuStat cpu, CpuSnapshot snapshot)
    {
        var total = cpu.Total - snapshot.Total;
        var active = cpu.Active - snapshot.Active;
        return total <= 0 ? 0 : (double)active / total * 100;
    }

    public static void LinuxMain()
    {
        var uptime = LinuxPlatform.GetUptime();
        Console.WriteLine(uptime.Uptime);

        var vm = LinuxPlatform.GetVirtualMemory();
        Console.WriteLine($"{vm.PageIn} {vm.PageOut} {vm.SwapIn} {vm.SwapOut} {vm.PageFault} {vm.MajorPageFault} {vm.OutOfMemoryKiller}");

        var fd = LinuxPlatform.GetFileDescriptor();
        Console.WriteLine($"{fd.Allocated} {fd.Used} {fd.Max}");

        var process = LinuxPlatform.GetProcessSummary();
        Console.WriteLine($"{process.ProcessCount} {process.ThreadCount}");

        var load = LinuxPlatform.GetLoadAverage();

        var stat = LinuxPlatform.GetStat();

        Console.WriteLine(stat.ProcessRunning);
        Console.WriteLine(stat.ProcessBlocked);
        Console.WriteLine(stat.ContextSwitch);

        var statSnapshot = new StatSnapshot();
        for (var i = 0; i < 10; i++)
        {
            Thread.Sleep(1000);

            statSnapshot.SnapshotFrom(stat);
            stat.Update();

            Console.WriteLine($"{stat.ContextSwitch} {stat.Interrupt} {stat.SoftIrq}");

            var contextSwitchPerSecond = (stat.ContextSwitch - statSnapshot.ContextSwitch) / (stat.UpdateAt - statSnapshot.UpdateAt).TotalSeconds;
            var interruptPerSecond = (stat.Interrupt - statSnapshot.Interrupt) / (stat.UpdateAt - statSnapshot.UpdateAt).TotalSeconds;
            var softIrqPerSecond = (stat.SoftIrq - statSnapshot.SoftIrq) / (stat.UpdateAt - statSnapshot.UpdateAt).TotalSeconds;
            Console.WriteLine($"{contextSwitchPerSecond:F2} {interruptPerSecond:F2} {softIrqPerSecond:F2}");
        }

        var cpuSnapshot = new CpuSnapshot();
        var cpuSnapshots = new CpuSnapshot[stat.Cpu.Count];
        for (var i = 0; i < cpuSnapshots.Length; i++)
        {
            cpuSnapshots[i] = new CpuSnapshot();
        }

        Console.WriteLine("Total    Cpu1   Cpu2   Cpu3   Cpu4");
        for (var i = 0; i < 100; i++)
        {
            Thread.Sleep(1000);

            cpuSnapshot.SnapshotFrom(stat.CpuTotal);
            for (var j = 0; j < cpuSnapshots.Length; j++)
            {
                cpuSnapshots[j].SnapshotFrom(stat.Cpu[j]);
            }
            stat.Update();
            load.Update();

            var sb = new StringBuilder();
            sb.Append(CalcUsage(stat.CpuTotal, cpuSnapshot).ToString("F2", CultureInfo.InvariantCulture).PadLeft(7));
            for (var j = 0; j < cpuSnapshots.Length; j++)
            {
                sb.Append(CalcUsage(stat.Cpu[j], cpuSnapshots[j]).ToString("F2", CultureInfo.InvariantCulture).PadLeft(7));
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
