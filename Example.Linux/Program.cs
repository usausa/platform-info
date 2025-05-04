using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

using PlatformInfo.Linux;

#pragma warning disable CA1416
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

var rootCommand = new RootCommand("Platform info");

//--------------------------------------------------------------------------------
// Uptime
//--------------------------------------------------------------------------------
var uptimeCommand = new Command("uptime", "Get uptime");
uptimeCommand.Handler = CommandHandler.Create(static () =>
{
    var uptime = LinuxPlatform.GetUptime();
    Console.WriteLine($"Uptime: {uptime.Uptime}");
});
rootCommand.Add(uptimeCommand);

//--------------------------------------------------------------------------------
// Statics
//--------------------------------------------------------------------------------
var statCommand = new Command("stat", "Get statics");
statCommand.Handler = CommandHandler.Create(static () =>
{
    var statics = LinuxPlatform.GetStatics();
    Console.WriteLine($"Interrupt:     {statics.Interrupt}");
    Console.WriteLine($"ContextSwitch: {statics.ContextSwitch}");
    Console.WriteLine($"SoftIrq:       {statics.SoftIrq}");
    // TODO
});
rootCommand.Add(statCommand);

//--------------------------------------------------------------------------------
// LoadAverage
//--------------------------------------------------------------------------------
var loadCommand = new Command("load", "Get load average");
loadCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(loadCommand);

//--------------------------------------------------------------------------------
// Memory
//--------------------------------------------------------------------------------
var memoryCommand = new Command("memory", "Get memory");
memoryCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(memoryCommand);

//--------------------------------------------------------------------------------
// VirtualMemory
//--------------------------------------------------------------------------------
var virtualCommand = new Command("virtual", "Get virtual memory");
virtualCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(virtualCommand);

//--------------------------------------------------------------------------------
// Partition
//--------------------------------------------------------------------------------
var partitionCommand = new Command("partition", "Get parition");
partitionCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(partitionCommand);

//--------------------------------------------------------------------------------
// DiskStatics
//--------------------------------------------------------------------------------
var diskCommand = new Command("disk", "Get disk statics");
diskCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(diskCommand);

//--------------------------------------------------------------------------------
// FileDescriptor
//--------------------------------------------------------------------------------
var fdCommand = new Command("fd", "Get file descriptor");
fdCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(fdCommand);

//--------------------------------------------------------------------------------
// Tcp
//--------------------------------------------------------------------------------
var tcpCommand = new Command("tcp", "Get tcp");
tcpCommand.Handler = CommandHandler.Create(static () =>
{
    var tcp = LinuxPlatform.GetTcp();
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
});
rootCommand.Add(tcpCommand);

//--------------------------------------------------------------------------------
// Tcp6
//--------------------------------------------------------------------------------
var tcp6Command = new Command("tcp6", "Get tcp6");
tcp6Command.Handler = CommandHandler.Create(static () =>
{
    var tcp = LinuxPlatform.GetTcp6();
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
});
rootCommand.Add(tcp6Command);

//--------------------------------------------------------------------------------
// ProcessSummary
//--------------------------------------------------------------------------------
var processCommand = new Command("process", "Get process");
processCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(processCommand);

//--------------------------------------------------------------------------------
// Cpu
//--------------------------------------------------------------------------------
var cpuCommand = new Command("cpu", "Get cpu");
cpuCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(cpuCommand);

//--------------------------------------------------------------------------------
// Battery
//--------------------------------------------------------------------------------
var batteryCommand = new Command("battery", "Get battery");
batteryCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(batteryCommand);

//--------------------------------------------------------------------------------
// MainsAdapter
//--------------------------------------------------------------------------------
var acCommand = new Command("ac", "Get ac");
acCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(acCommand);

//--------------------------------------------------------------------------------
// HardwareMonitor
//--------------------------------------------------------------------------------
var hwmonCommand = new Command("hwmon", "Get hwmon");
hwmonCommand.Handler = CommandHandler.Create(static () =>
{
    // TODO
});
rootCommand.Add(hwmonCommand);

//--------------------------------------------------------------------------------
// Run
//--------------------------------------------------------------------------------
rootCommand.Invoke(["tcp6"]);
//rootCommand.Invoke(args);
