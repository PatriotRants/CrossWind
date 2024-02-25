namespace ForgeWorks.CrossWind;

public interface IIdentifier
{
    /// <summary>
    /// Get the identifier Id
    /// </summary>
    string Id { get; }
    /// <summary>
    /// Get the identifier Name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Get the identifier Type
    /// </summary>
    Type Type { get; }
}
