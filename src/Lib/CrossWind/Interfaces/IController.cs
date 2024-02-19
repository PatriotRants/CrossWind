namespace ForgeWorks.CrossWind.Core;

public delegate void OnControllerDisposing(IIdentifier controllerId);

public interface IController : INamed, IDisposable, IUnique
{
    event OnControllerDisposing OnDisposing;

    public int Handle { get; }
    public Type Type { get; }


    /// <summary>
    /// Initializes <see cref="Controller"/> resources
    /// </summary>
    void Initialize();
}
