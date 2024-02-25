namespace ForgeWorks.CrossWind.Core;

public interface IApplication : INamed, IDisposable
{
    /// <summary>
    /// Get the CrossWind assembly version
    /// </summary>
    Version CrossWindVersion { get; }
    /// <summary>
    /// Get the current Application's Controller
    /// </summary>
    IApplicationController Controller { get; }
}
