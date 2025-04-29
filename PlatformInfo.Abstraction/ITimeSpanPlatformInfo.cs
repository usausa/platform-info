namespace PlatformInfo.Abstraction;

public interface ITimeSpanPlatformInfo : IPlatformInfo
{
    TimeSpan TimeSpan { get; }
}
