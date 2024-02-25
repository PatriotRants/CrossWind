namespace ForgeWorks.CrossWind.Core;

/// <summary>
/// Uniquely identified object
/// </summary>
public interface IUnique
{
    /// <summary>
    /// Get the object's identifier
    /// </summary>
    /// <returns><see cref="IIdentifier"/></returns>
    IIdentifier GetIdentifier();
}
