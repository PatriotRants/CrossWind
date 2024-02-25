using ForgeWorks.CrossWind.Collections;

namespace ForgeWorks.CrossWind;

/// <summary>
/// Tyoe Indentifier
/// </summary>
public readonly struct TypeId : IIdentifier
{
    /// <summary>
    /// An Empty Type Identifier
    /// </summary>
    public static TypeId Empty { get; } = new(null);

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Id { get; init; } = Guid.Empty.Short();
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Name => Type.Name;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Initialize a new Type Identifier
    /// </summary>
    /// <param name="type"></param>
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

    /// <summary>
    /// <inheritdoc />
    /// </summary>
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
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is TypeId typeId)
        {
            return this == typeId;
        }

        return false;
    }
}
