namespace ForgeWorks.CrossWind.Core;

public abstract class Controller : IController
{
    public ControllerId Id { get; }

    public string Name => Id.Name;
    public int Handle => Id.GetHashCode();
    public Type Type => Id.Type;

    protected Controller(string name, Type type)
    {
        Id = new(type)
        {
            Name = name
        };
    }
}
