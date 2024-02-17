using System.Reflection;
using ForgeWorks.CrossWind.Core;

namespace ForgeWorks.CrossWind.Collections;

public static class Registries
{
    static Registries()
    {
        /*
            TODO: add default types from configuration file ...???
        */
        //  Add default types to registry
        Types.Add(typeof(ApplicationController));
    }

    public static IRegistry<IController> Controllers => Registry<Controller, IController>.Instance;
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

    //  Registry objects
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
        public bool Contains(TInterface item)
        {
            return registry.IndexOf((T)item) != -1;
        }
        public bool TryGet(Func<TInterface, bool> find, out TInterface controller)
        {
            controller = registry
                .Select(t => (TInterface)t)
                .FirstOrDefault(find);

            return controller != null;
        }

        protected bool Contains(Func<T, bool> find)
        {
            return registry.Any(find);
        }

        protected TInterface Find(Func<T, bool> find)
        {
            return registry.FirstOrDefault(find);
        }
    }

    private class TypeRegistry : IRegistry<TypeId>
    {
        private static readonly Lazy<TypeRegistry> _registry = new(() => new());

        private List<TypeId> registry = new();

        internal static TypeRegistry Instance => _registry.Value;

        public bool Add(TypeId item)
        {
            registry.Add(item);

            return Contains(item);
        }
        public bool Contains(TypeId item)
        {
            return registry.IndexOf(item) != -1;
        }
        public bool TryAdd(Type type, out TypeId typeId)
        {
            typeId = TypeId.Empty;

            if (!Contains(t => t.Type == type))
            {
                return Add(typeId = new(type)
                {
                    Id = Guid.NewGuid()
                });
            }

            return typeId != TypeId.Empty;
        }
        public bool TryGet(Func<TypeId, bool> find, out TypeId typeId)
        {
            typeId = Find(find);

            return typeId != TypeId.Empty;
        }
        public bool TryGetId(Type type, out TypeId typeId)
        {
            typeId = Find(t => t.Type == type);

            return typeId != TypeId.Empty;
        }

        protected bool Contains(Func<TypeId, bool> find)
        {
            return registry.Any(find);
        }
        protected TypeId Find(Func<TypeId, bool> find)
        {
            return registry.FirstOrDefault(find);
        }
    }
}
