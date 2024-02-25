namespace ForgeWorks.CrossWind.Core;

public delegate void OnControllerDisposing(IIdentifier controllerId);

/// <summary>
/// Base controller interface
/// </summary>
public interface IController : INamed, IDisposable, IUnique
{
    /// <summary>
    /// Event raised when the controller is about to dispose
    /// </summary>
    event OnControllerDisposing OnDisposing;

    /// <summary>
    /// Get the controller's handle
    /// </summary>
    public int Handle { get; }
    /// <summary>
    /// Gat the controller's implemented type
    /// </summary>
    public Type Type { get; }


    /// <summary>
    /// Initializes <see cref="Controller"/> resources
    /// </summary>
    void Initialize();
}
