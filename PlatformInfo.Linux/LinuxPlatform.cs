namespace PlatformInfo.Linux;

using System.Diagnostics;
using System.Runtime.Versioning;

#pragma warning disable CA1024
[SupportedOSPlatform("linux")]
public static class LinuxPlatform
{
    public static UptimeInfo GetUptimeInfo() => new();

    // TODO Cycleも初期化時に計算？、go方式？
    public static void CpuTest()
    {
        // StatInfo, CPU更新 CpuStat
        // Diff
        // 累計は前回にするか？
        // cpu  285 0 188 543 128 0 34 0 0 0
        // cpu0 71 0 44 118 30 0 26 0 0 0
        // cpu1 69 0 37 146 39 0 5 0 0 0
        // cpu2 77 0 53 144 24 0 3 0 0 0
        // cpu3 68 0 54 134 33 0 0 0 0 0
        //X intr 21894 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1 861 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0
        // ctxt 51948
        //X btime 1745312140
        //X processes 562
        // procs_running 4
        // procs_blocked 0
        //X softirq 30161 0 251 2 52 0 0 22750 968 0 6138

        using var reader = new StreamReader("/proc/stat");
        while (reader.ReadLine() is { } line)
        {
            Debug.WriteLine(line);
        }
        // TODO
    }

    public static MemoryInfo GetMemoryInfo() => new();
}
