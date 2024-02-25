using static MinuteMan.LabKit.TestSet;

namespace MinuteMan.LabKit;

#nullable disable
public abstract class Prototype<TPrototype> : Prototype
{
    protected Prototype()
    {
        Type = GetType();
    }

    public TResult Proto<TResult>(Func<TPrototype, TResult> func)
    {

        return default;
    }
}

public class Prototype
{
    private static TestContext Context => TestContext.Current;

    public Type Type { get; protected set; }


    /// <summary>
    /// Identifies a method that should be executed for prototyping
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PrototypeAttribute : Attribute
    {

        public PrototypeAttribute() { }
    }

    public static bool Evaluate<TPrototype>(Func<TPrototype, bool> evaluate, string name, string description) where TPrototype : Prototype
    {
        Log($"[Eval] Evaluate: <{typeof(TPrototype).Name}> {name}:{description}");

        //  get prototype
        var ctor = Context.Get<TPrototype>();
        //  assumption: parameterless constructor
        var param = new object[] { };
        var prototype = (TPrototype)ctor.Invoke(param);

        var result = evaluate(prototype);

        Log("=========================================================\n");

        return result;
    }
    public static void Evaluate<TPrototype>(Action<TPrototype> evaluate, string name, string description) where TPrototype : Prototype
    {
        Log($"[Eval] Evaluate: <{typeof(TPrototype).Name}> {name}:{description}");

        //  get prototype
        var ctor = Context.Get<TPrototype>();
        //  assumption: parameterless constructor
        var param = new object[] { };
        var prototype = (TPrototype)ctor.Invoke(param);

        evaluate(prototype);

        Log("=========================================================\n");
    }
}
