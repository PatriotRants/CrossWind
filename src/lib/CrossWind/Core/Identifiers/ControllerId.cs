using ForgeWorks.CrossWind.Collections;

namespace ForgeWorks.CrossWind;

/// <summary>
/// Controller Identifier
/// </summary>
public readonly struct ControllerId : IIdentifier
{
    private readonly TypeId typeId;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Id => typeId.Id;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Type Type => typeId.Type;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Initialize a new Controller Identifier
    /// </summary>
    /// <param name="type"></param>
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


    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override int GetHashCode()
    {
        int hash = Id.GetHashCode();

        unchecked
        {
            hash *= Name.GetHashCode() + 3109;
        }

        return hash;
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is ControllerId controllerId)
        {
            return this == controllerId;
        }

        return false;
    }
}
