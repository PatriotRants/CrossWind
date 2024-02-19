namespace ForgeWorks.CrossWind;

public interface IIdentifier
{
    Guid Id { get; }
    string Name { get; }
    Type Type { get; }
}
