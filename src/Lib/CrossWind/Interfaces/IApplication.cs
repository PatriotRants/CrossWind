namespace ForgeWorks.CrossWind.Core;

public interface IApplication : INamed
{
    Version CrossWindVersion { get; }
    IApplicationController Controller { get; }
}
