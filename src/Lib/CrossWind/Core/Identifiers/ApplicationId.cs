using ForgeWorks.CrossWind.Collections;

namespace ForgeWorks.CrossWind;

public readonly struct ApplicationId : IIdentifier
{
    private readonly TypeId typeId;

    public Guid Id => typeId.Id;
    public Type Type => typeId.Type;
    public string Name { get; init; }

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
        if (obj is ApplicationId applicationId)
        {
            return this == applicationId;
        }

        return false;
    }

}