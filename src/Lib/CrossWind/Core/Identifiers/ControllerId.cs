using ForgeWorks.CrossWind.Collections;

namespace ForgeWorks.CrossWind;

public struct ControllerId
{
    private readonly TypeId typeId;

    public Guid Id => typeId.Id;
    public Type Type => typeId.Type;
    public string Name { get; init; }

    public ControllerId(Type type)
    {
        if (!Registries.Types.TryGet(t => t.Type == type, out typeId))
        {
            typeId = Registries.Types.Add(type);
        }
    }


    public static bool operator ==(ControllerId id1, ControllerId id2)
    {
        return id1.Id == id2.Id && id1.Name == id2.Name;
    }
    public static bool operator !=(ControllerId id1, ControllerId id2)
    {
        return id1.Id != id2.Id || id1.Name != id2.Name;
    }

    public override int GetHashCode()
    {
        int hash = Id.GetHashCode();

        unchecked
        {
            hash *= Name.GetHashCode() + 3109;
        }

        return hash;
    }

    public override bool Equals(object obj)
    {
        if (obj is ControllerId controllerId)
        {
            return this == controllerId;
        }

        return false;
    }
}
