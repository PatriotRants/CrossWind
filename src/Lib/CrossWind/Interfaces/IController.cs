namespace ForgeWorks.CrossWind.Core;

public delegate void OnControllerDisposing(ControllerId controllerId);

public interface IController : INamed, IDisposable
{
    event OnControllerDisposing OnDisposing;

    public int Handle { get; }
    public Type Type { get; }
}
