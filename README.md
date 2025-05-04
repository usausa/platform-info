# platform-info

## Linux

[![NuGet](https://img.shields.io/nuget/v/PlatformInfo.Linux.svg)](https://www.nuget.org/packages/PlatformInfo.Linux)

### Uptime

```csharp
var uptime = LinuxPlatform.GetUptime();
Console.WriteLine($"Uptime: {uptime.Uptime}");
```

### Statics

```csharp
var statics = LinuxPlatform.GetStatics();
Console.WriteLine($"Interrupt:      {statics.Interrupt}");
Console.WriteLine($"ContextSwitch:  {statics.ContextSwitch}");
Console.WriteLine($"SoftIrq:        {statics.SoftIrq}");
Console.WriteLine($"ProcessRunning: {statics.ProcessRunning}");
Console.WriteLine($"ProcessBlocked: {statics.ProcessBlocked}");

Console.WriteLine($"User:           {statics.CpuTotal.User}");
Console.WriteLine($"Nice:           {statics.CpuTotal.Nice}");
Console.WriteLine($"System:         {statics.CpuTotal.System}");
Console.WriteLine($"Idle:           {statics.CpuTotal.Idle}");
Console.WriteLine($"IoWait:         {statics.CpuTotal.IoWait}");
Console.WriteLine($"Irq:            {statics.CpuTotal.Irq}");
Console.WriteLine($"SoftIrq:        {statics.CpuTotal.SoftIrq}");
Console.WriteLine($"Steal:          {statics.CpuTotal.Steal}");
Console.WriteLine($"Guest:          {statics.CpuTotal.Guest}");
Console.WriteLine($"GuestNice:      {statics.CpuTotal.GuestNice}");
```

### LoadAverage

```csharp
var load = LinuxPlatform.GetLoadAverage();
Console.WriteLine($"Average1:  {load.Average1:F2}");
Console.WriteLine($"Average5:  {load.Average5:F2}");
Console.WriteLine($"Average15: {load.Average15:F2}");
```

### Memory

```csharp
var memory = LinuxPlatform.GetMemory();
Console.WriteLine($"Total:   {memory.Total}");
Console.WriteLine($"Free:    {memory.Free}");
Console.WriteLine($"Buffers: {memory.Buffers}");
Console.WriteLine($"Cached:  {memory.Cached}");
Console.WriteLine($"Usage:   {(int)Math.Ceiling(memory.Usage)}");
```

### VirtualMemory

```csharp
var vm = LinuxPlatform.GetVirtualMemory();
Console.WriteLine($"PageIn:            {vm.PageIn}");
Console.WriteLine($"PageOut:           {vm.PageOut}");
Console.WriteLine($"SwapIn:            {vm.SwapIn}");
Console.WriteLine($"SwapOut:           {vm.SwapOut}");
Console.WriteLine($"PageFault:         {vm.PageFault}");
Console.WriteLine($"MajorPageFault:    {vm.MajorPageFault}");
Console.WriteLine($"OutOfMemoryKiller: {vm.OutOfMemoryKiller}");
```

### Partition

```csharp
var partitions = LinuxPlatform.GetPartitions();
foreach (var partition in partitions)
{
    var drive = new DriveInfo(partition.MountPoints[0]);
    var used = drive.TotalSize - drive.TotalFreeSpace;
    var available = drive.AvailableFreeSpace;
    var usage = (int)Math.Ceiling((double)used / (available + used) * 100);

    Console.WriteLine($"Name:          {partition.Name}");
    Console.WriteLine($"MountPoint:    {String.Join(' ', partition.MountPoints)}");
    Console.WriteLine($"TotalSize:     {drive.TotalSize / 1024}");
    Console.WriteLine($"UsedSize:      {used / 1024}");
    Console.WriteLine($"AvailableSize: {available / 1024}");
    Console.WriteLine($"Usage:         {usage}");
}
```

### DiskStatics

```csharp
var disk = LinuxPlatform.GetDiskStatics();
foreach (var device in disk.Devices)
{
    Console.WriteLine($"Name:           {device.Name}");
    Console.WriteLine($"ReadCompleted:  {device.ReadCompleted}");
    Console.WriteLine($"ReadMerged:     {device.ReadMerged}");
    Console.WriteLine($"ReadSectors:    {device.ReadSectors}");
    Console.WriteLine($"ReadTime:       {device.ReadTime}");
    Console.WriteLine($"WriteCompleted: {device.WriteCompleted}");
    Console.WriteLine($"WriteMerged:    {device.WriteMerged}");
    Console.WriteLine($"WriteSectors:   {device.WriteSectors}");
    Console.WriteLine($"WriteTime:      {device.WriteTime}");
    Console.WriteLine($"IosInProgress:  {device.IosInProgress}");
    Console.WriteLine($"IoTime:         {device.IoTime}");
    Console.WriteLine($"WeightIoTime:   {device.WeightIoTime}");
}
```

### FileDescriptor

```csharp
var fd = LinuxPlatform.GetFileDescriptor();
Console.WriteLine($"Allocated: {fd.Allocated}");
Console.WriteLine($"Used:      {fd.Used}");
Console.WriteLine($"Max:       {fd.Max}");
```

### NetworkStatic

```csharp
var network = LinuxPlatform.GetNetworkStatic();
foreach (var nif in network.Interfaces)
{
    Console.WriteLine($"Interface:    {nif.Interface}");
    Console.WriteLine($"RxBytes:      {nif.RxBytes}");
    Console.WriteLine($"RxPackets:    {nif.RxPackets}");
    Console.WriteLine($"RxErrors:     {nif.RxErrors}");
    Console.WriteLine($"RxDropped:    {nif.RxDropped}");
    Console.WriteLine($"RxFifo:       {nif.RxFifo}");
    Console.WriteLine($"RxFrame:      {nif.RxFrame}");
    Console.WriteLine($"RxCompressed: {nif.RxCompressed}");
    Console.WriteLine($"RxMulticast:  {nif.RxMulticast}");
    Console.WriteLine($"TxBytes:      {nif.TxBytes}");
    Console.WriteLine($"TxPackets:    {nif.TxPackets}");
    Console.WriteLine($"TxErrors:     {nif.TxErrors}");
    Console.WriteLine($"TxDropped:    {nif.TxDropped}");
    Console.WriteLine($"TxFifo:       {nif.TxFifo}");
    Console.WriteLine($"TxCollisions: {nif.TxCollisions}");
    Console.WriteLine($"TxCarrier:    {nif.TxCarrier}");
    Console.WriteLine($"TxCompressed: {nif.TxCompressed}");
}
```

### Tcp/Tcp6

```csharp
var tcp = LinuxPlatform.GetTcp();
var tcp6 = LinuxPlatform.GetTcp6();
Console.WriteLine($"Established: {tcp.Established}");
Console.WriteLine($"SynSent:     {tcp.SynSent}");
Console.WriteLine($"SynRecv:     {tcp.SynRecv}");
Console.WriteLine($"FinWait1:    {tcp.FinWait1}");
Console.WriteLine($"FinWait2:    {tcp.FinWait2}");
Console.WriteLine($"TimeWait:    {tcp.TimeWait}");
Console.WriteLine($"Close:       {tcp.Close}");
Console.WriteLine($"CloseWait:   {tcp.CloseWait}");
Console.WriteLine($"LastAck:     {tcp.LastAck}");
Console.WriteLine($"Listen:      {tcp.Listen}");
Console.WriteLine($"Closing:     {tcp.Closing}");
Console.WriteLine($"Total:       {tcp.Total}");
```

### ProcessSummary

```csharp
var process = LinuxPlatform.GetProcessSummary();
Console.WriteLine($"ProcessCount: {process.ProcessCount}");
Console.WriteLine($"ThreadCount:  {process.ThreadCount}");
```

### Cpu

```csharp
var cpu = LinuxPlatform.GetCpu();
Console.WriteLine("Frequency");
foreach (var core in cpu.Cores)
{
    Console.WriteLine($"{core.Name}: {core.Frequency}");
}

if (cpu.Powers.Count > 0)
{
    Console.WriteLine("Power");
    foreach (var power in cpu.Powers)
    {
        Console.WriteLine($"{power.Name}: {power.Energy / 1000.0}");
    }
}
```


### Battery

```csharp
var battery = LinuxPlatform.GetBattery();
if (battery.Supported)
{
    Console.WriteLine($"Capacity:   {battery.Capacity}");
    Console.WriteLine($"Status:     {battery.Status}");
    Console.WriteLine($"Voltage:    {battery.Voltage / 1000.0:F2}");
    Console.WriteLine($"Current:    {battery.Current / 1000.0:F2}");
    Console.WriteLine($"Charge:     {battery.Charge / 1000.0:F2}");
    Console.WriteLine($"ChargeFull: {battery.ChargeFull / 1000.0:F2}");
}
else
{
    Console.WriteLine("No battery found");
}
```

### MainsAdapter

```csharp
var adapter = LinuxPlatform.GetMainsAdapter();
if (adapter.Supported)
{
    Console.WriteLine($"Online: {adapter.Online}");
}
else
{
    Console.WriteLine("No adapter found");
}
```

### HardwareMonitor

```csharp
var monitors = LinuxPlatform.GetHardwareMonitors();
foreach (var monitor in monitors)
{
    Console.WriteLine($"Monitor: {monitor.Type}");
    Console.WriteLine($"Name:    {monitor.Name}");
    foreach (var sensor in monitor.Sensors)
    {
        Console.WriteLine($"Sensor:  {sensor.Type}");
        Console.WriteLine($"Label:   {sensor.Label}");
        Console.WriteLine($"Value:   {sensor.Value}");
    }
}
```

### TODO

- [ ] SMART?
- [ ] Docker

## Darwin

(TODO)
