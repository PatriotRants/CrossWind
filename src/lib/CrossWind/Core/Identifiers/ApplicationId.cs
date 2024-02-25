using ForgeWorks.CrossWind.Collections;

namespace ForgeWorks.CrossWind;

/// <summary>
/// Application Identifier
/// </summary>
public readonly struct ApplicationId : IIdentifier
{
    private readonly TypeId typeId;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Id => typeId.Id;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Name { get; init; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Type Type => typeId.Type;

    /// <summary>
    /// Initialize a new Application Identifier
    /// </summary>
    /// <param name="type"></param>
    public ApplicationId(Type type)
    {
        typeId = Registries.Types.Add(type);
    }

    public static bool operator ==(ApplicationId id1, ApplicationId id2)
    {
        return id1.Id == id2.Id && id1.Name == id2.Name;
    }
    public static bool operator !=(ApplicationId id1, ApplicationId id2)
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
        if (obj is ApplicationId applicationId)
        {
            return this == applicationId;
        }

        return false;
    }

}