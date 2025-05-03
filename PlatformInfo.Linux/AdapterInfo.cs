namespace PlatformInfo.Linux;

using System;

using PlatformInfo.Abstraction;

public sealed class AdapterInfo : IPlatformInfo
{
    private const string PowerSupplyPath = "/sys/class/power_supply/";

    private readonly string path;

    public DateTime UpdateAt { get; private set; }

    public bool Supported => !String.IsNullOrEmpty(path);

    public bool Online { get; private set; }

    internal AdapterInfo()
    {
        path = FindAdapter();
        Update();
    }

    public bool Update()
    {
        if (!Supported)
        {
            return false;
        }

        var now = DateTime.Now;
        if (UpdateAt == now)
        {
            return true;
        }

        Online = ReadFile("online").AsSpan().StartsWith("1");

        UpdateAt = now;

        return true;
    }

    private static string FindAdapter()
    {
        if (Directory.Exists(PowerSupplyPath))
        {
            foreach (var dir in Directory.GetDirectories(PowerSupplyPath))
            {
                var file = Path.Combine(dir, "type");
                if (File.Exists(file))
                {
                    var type = File.ReadAllText(file).AsSpan().Trim();
                    if (type.StartsWith("Mains", StringComparison.OrdinalIgnoreCase))
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
}
