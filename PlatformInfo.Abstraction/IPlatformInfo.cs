namespace PlatformInfo.Abstraction;

public interface IPlatformInfo
{
    DateTime UpdateAt { get; }

    bool Update();
}
