namespace ForgeWorks.CrossWind;

public readonly struct TypeId : IIdentifier
{
    public static TypeId Empty { get; } = new(null);

    public Guid Id { get; init; } = Guid.Empty;

    public Type Type { get; }
    public string Name => Type.Name;

    public TypeId(Type type)
    {
        Type = type;
    }

    public static bool operator ==(TypeId id1, TypeId id2)
    {
        return id1.Id == id2.Id && id1.Type == id2.Type;
    }
    public static bool operator !=(TypeId id1, TypeId id2)
    {
        return id1.Id != id2.Id || id1.Type != id2.Type;
    }

    public override int GetHashCode()
    {
        int hash = 3089;

        unchecked
        {
            hash *= Id.GetHashCode() + 1609;
            hash *= Type.GetHashCode() + 5689;
        }

        return hash;
    }

    public override bool Equals(object obj)
    {
        if (obj is TypeId typeId)
        {
            return this == typeId;
        }

        return false;
    }
}
