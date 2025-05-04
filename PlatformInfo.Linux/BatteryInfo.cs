namespace PlatformInfo.Linux;

using System;

using PlatformInfo.Abstraction;

public sealed class BatteryInfo : IPlatformInfo
{
    private const string PowerSupplyPath = "/sys/class/power_supply/";

    private readonly string path;

    public DateTime UpdateAt { get; private set; }

    public bool Supported => !String.IsNullOrEmpty(path);

    public int Capacity { get; private set; }

    public string Status { get; private set; } = string.Empty;

    // mV
    public double Voltage { get; private set; }

    // mA
    public double Current { get; private set; }

    // mAh
    public double Charge { get; private set; }

    // mAh
    public double ChargeFull { get; private set; }

    internal BatteryInfo()
    {
        path = FindBattery();
        Update();
    }

    public bool Update()
    {
        if (!Supported)
        {
            return false;
        }

        Capacity = ReadFileAsInt32("capacity");
        Status = ReadFile("status");
        Voltage = ReadFileAsMilliDouble("voltage_now");
        Current = ReadFileAsMilliDouble("current_now");
        Charge = ReadFileAsMilliDouble("charge_now");
        ChargeFull = ReadFileAsMilliDouble("charge_full");

        UpdateAt = DateTime.Now;

        return true;
    }

    private static string FindBattery()
    {
        if (Directory.Exists(PowerSupplyPath))
        {
            foreach (var dir in Directory.GetDirectories(PowerSupplyPath))
            {
                var file = Path.Combine(dir, "type");
                if (File.Exists(file))
                {
                    var type = File.ReadAllText(file).AsSpan().Trim();
                    if (type.StartsWith("Battery", StringComparison.OrdinalIgnoreCase))
                    {
                        return dir;
                    }
                }
            }
        }

        return string.Empty;
    }

    private string ReadFile(string name)
    {
        var file = Path.Combine(path, name);
        if (File.Exists(file))
        {
            return File.ReadAllText(file);
        }

        return string.Empty;
    }

    private int ReadFileAsInt32(string name)
    {
        var value = ReadFile(name);
        if (String.IsNullOrEmpty(value))
        {
            return 0;
        }

        return Int32.TryParse(value, out var result) ? result : 0;
    }

    private double ReadFileAsMilliDouble(string name)
    {
        var value = ReadFile(name);
        if (String.IsNullOrEmpty(value))
        {
            return 0;
        }

        return Double.TryParse(value, out var result) ? result / 1000 : 0;
    }
}
