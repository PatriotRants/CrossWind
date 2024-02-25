using System.Text;
using System.Reflection;

using ForgeWorks.CrossWind.Core;

namespace ForgeWorks.CrossWind.Collections;

/// <summary>
/// Core registries
/// </summary>
public static class Registries
{
    /// <summary>
    /// Abbreviates a GUID to the last 6 bytes
    /// </summary>
    /// <param name="guid">The GUID to abbreviate</param>
    /// <returns>The abbreviated byte string</returns>
    public static string Short(this Guid guid)
    {
        //  take last 6 bytes
        byte[] bytes = guid.ToByteArray()
            .TakeLast(6)
            .ToArray();

        return BitConverter.ToString(bytes)
            .Replace("-", string.Empty)
            .ToUpper();
    }

    static Registries()
    {
        /*
            TODO: add default types from configuration file ...???
        */
        //  Add default types to registry
        Types.Add(typeof(ApplicationController));
    }

    /// <summary>
    /// The Controllers Registry
    /// </summary>
    public static IRegistry<IController> Controllers => Registry<Controller, IController>.Instance;
    /// <summary>
    /// The Types Registry
    /// </summary>
    public static IRegistry<TypeId> Types = TypeRegistry.Instance;

    //  Extension methods for specific registries
    /// <summary>
    /// Determines whether the Type Registry contains the given type
    /// </summary>
    /// <param name="registry">Type Registry</param>
    /// <param name="type">Type to verify</param>
    /// <returns>TRUE if Registry contains type; otherwise FALSE</returns>
    public static bool Contains(this IRegistry<TypeId> registry, Type type)
    {
        return ((TypeRegistry)registry).TryGetId(type, out TypeId _);
    }
    /// <summary>
    /// Adds non-duplicate Type to Type Registry
    /// </summary>
    /// <param name="registry">Type Registry</param>
    /// <param name="type">Type to add</param>
    /// <returns>Valid TypeId if not duplicate; otherwise DEFAULT</returns>
    public static TypeId Add(this IRegistry<TypeId> registry, Type type)
    {
        ((TypeRegistry)registry).TryAdd(type, out TypeId typeId);

        return typeId;
    }
    /// <summary>
    /// Creates the controller object
    /// </summary>
    /// <typeparam name="TController">Controller type</typeparam>
    /// <param name="registry">Registry</param>
    /// <param name="name">Controller name</param>
    /// <returns>Controller</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TController Create<TController>(this IRegistry<IController> registry, string name) where TController : IController
    {
        //  get controller type
        var controllerType = typeof(TController);
        //  expect constructor with name parameter
        var expectedTypes = new Type[] { typeof(string) };
        var ctor = controllerType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, expectedTypes);

        if (ctor == null)
        {
            throw new ArgumentNullException(nameof(ctor));
        }

        var parameters = new string[] { name };
        var controller = (TController)ctor.Invoke(parameters);

        registry.Add(controller);

        return controller;
    }
    /// <summary>
    /// Get the controller
    /// </summary>
    /// <typeparam name="TController">Controller type</typeparam>
    /// <param name="registry">Registry</param>
    /// <param name="name">Controller name</param>
    /// <returns>Controller; null if not found</returns>
    public static TController Get<TController>(this IRegistry<IController> registry, string name)
    {
        registry.TryGet(c =>
            c.Type == typeof(TController) && c.Name == name,
            out IController controller);

        return (TController)controller;
    }
    /// <summary>
    /// Kills specified controllers of given type calling dispose.
    /// </summary>
    /// <typeparam name="TController">Controller type</typeparam>
    /// <param name="registry">Registry</param>
    /// <returns>TRUE if all items were removed; otherwise FALSE</returns>
    public static bool Kill<TController>(this IRegistry<IController> registry)
    {
        var success = true;

        // This method will kill all controllers of the given type
        var controllers = registry.GetAll(typeof(TController))
            .ToArray();

        foreach (var c in controllers)
        {
            c.Dispose();
            success &= registry.Remove(c);
        }

        return success;
    }
    /// <summary>
    /// Retrieves all Controllers of <see cref="TController"/> 
    /// </summary>
    /// <typeparam name="TController"><see cref="TController"/></typeparam>
    /// <param name="registry">Current registry</param>
    /// <returns>The <see cref="IController"/> interface</returns>
    public static IEnumerable<IController> GetAll<TController>(this IRegistry<IController> registry)
    {
        return registry.GetAll(typeof(TController));
    }

    /// <summary>
    /// Generic Registry
    /// </summary>
    private class Registry<T, TInterface> : IRegistry<TInterface> where T : TInterface
    {
        private static readonly Lazy<Registry<T, TInterface>> _registry = new(() => new());

        private List<T> registry = new();

        internal static Registry<T, TInterface> Instance => _registry.Value;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public virtual bool Add(TInterface item)
        {
            registry.Add((T)item);

            return Contains(item);
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool Contains(TInterface item)
        {
            return registry.IndexOf((T)item) != -1;
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IEnumerable<TInterface> GetAll(Type type)
        {
            return registry
                .Select(t => (TInterface)t)
                .Where(t => type.IsAssignableFrom(t.GetType()));
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool Remove(TInterface item)
        {
            return registry.Remove((T)item);
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <returns></returns>
        public bool TryGet(Func<TInterface, bool> find, out TInterface controller)
        {
            controller = registry
                .Select(t => (TInterface)t)
                .FirstOrDefault(find);

            return controller != null;
        }
        /// <summary>
        /// Determine by predicate search criteria whether specified item is in the registry
        /// </summary>
        /// <param name="find">Search criteria</param>
        /// <returns>TRUE if found/ otherwise FALSE</returns>
        protected bool Contains(Func<T, bool> find)
        {
            return registry.Any(find);
        }
        /// <summary>
        /// Find and retrieve specified item by predicate search criteria
        /// </summary>
        /// <param name="find">Search criteria</param>
        /// <returns>The found item; if not found NULL</returns>
        protected TInterface Find(Func<T, bool> find)
        {
            return registry.FirstOrDefault(find);
        }
    }
    /// <summary>
    /// Types Registry
    /// </summary>
    private class TypeRegistry : IRegistry<TypeId>
    {
        private static readonly Lazy<TypeRegistry> _registry = new(() => new());

        private List<TypeId> registry = new();

        internal static TypeRegistry Instance => _registry.Value;
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool Add(TypeId item)
        {
            registry.Add(item);

            return Contains(item);
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool Contains(TypeId item)
        {
            return registry.IndexOf(item) != -1;
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IEnumerable<TypeId> GetAll(Type type)
        {
            return registry
                .Where(t => t.Type == type);
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool Remove(TypeId item)
        {
            return registry.Remove(item);
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool TryAdd(Type type, out TypeId typeId)
        {
            typeId = TypeId.Empty;

            if (!Contains(t => t.Type == type))
            {
                return Add(typeId = new(type)
                {
                    Id = Guid.NewGuid().Short()
                });
            }

            return typeId != TypeId.Empty;
        }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool TryGet(Func<TypeId, bool> find, out TypeId typeId)
        {
            typeId = Find(find);

            return typeId != TypeId.Empty;
        }
        /// <summary>
        /// Indicates whether a specified <see cref="Type"/> is in the Types registry
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to find</param>
        /// <param name="typeId">Found <see cref="TypeId"/>; otherwise NULL</param>
        /// <returns>TRUE if found; otherwise FALSE</returns>
        public bool TryGetId(Type type, out TypeId typeId)
        {
            typeId = Find(t => t.Type == type);

            return typeId != TypeId.Empty;
        }
        /// <summary>
        /// Determine by predicate search criteria whether specified <see cref="TypeID"/> is in the registry
        /// </summary>
        /// <param name="find">Search criteria</param>
        /// <returns>TRUE if found/ otherwise FALSE</returns>
        protected bool Contains(Func<TypeId, bool> find)
        {
            return registry.Any(find);
        }
        /// <summary>
        /// Find and retrieve specified item by predicate search criteria
        /// </summary>
        /// <param name="find">Search criteria</param>
        /// <returns>The found <see cref="TypeId">; if not found NULL</returns>
        protected TypeId Find(Func<TypeId, bool> find)
        {
            return registry.FirstOrDefault(find);
        }
    }
}
