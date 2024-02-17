namespace ForgeWorks.CrossWind.Core;

public interface IController : INamed
{
    public int Handle { get; }
    public Type Type { get; }
}
