using ForgeWorks.CrossWind.Core;

namespace ForgeWorks.CrossWind.Collections;

public interface IRegistry<TInterface>
{
    /// <summary>
    /// Add <cref name="TInterface" /> to Registry
    /// </summary>
    bool Add(TInterface item);
    /// <summary>
    /// Determines if a specified item is in the registry
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool Contains(TInterface item);
    /// <summary>
    /// Retireves all items of the specified <see cref="TInterface"/>
    /// </summary>
    /// <param name="type">Type to query</param>
    /// <returns><see cref="IEnumerable"/> of <see cref="TInterface"/></returns>
    IEnumerable<TInterface> GetAll(Type type);
    /// <summary>
    /// Removes the spedified item from the registry
    /// </summary>
    /// <param name="item">The item to remove</param>
    /// <returns>TRUE if removed; otherwise FALSE</returns>
    bool Remove(TInterface item);
    /// <summary>
    /// Indicates whether a predicated item could be retrieved
    /// </summary>
    /// <param name="find">Predicate search criteria</param>
    /// <param name="item">Found item; otherwise NULL</param>
    /// <returns>TRUE if found; otherwise FALSe</returns>
    bool TryGet(Func<TInterface, bool> find, out TInterface item);
}
