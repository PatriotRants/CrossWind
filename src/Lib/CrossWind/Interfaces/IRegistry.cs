namespace ForgeWorks.CrossWind.Collections;

public interface IRegistry<TInterface>
{
    /// <summary>
    /// Add <cref name="TInterface" /> to Registry
    /// </summary>
    bool Add(TInterface item);
    bool Contains(TInterface item);
    bool TryGet(Func<TInterface, bool> find, out TInterface item);
}
