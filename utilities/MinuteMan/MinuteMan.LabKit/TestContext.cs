using System.Reflection;

namespace MinuteMan.LabKit;


#nullable disable
public class TestContext
{
    private static readonly Lazy<TestContext> lazyContext = new(() => new());

    private Queue<string> ResultsLog { get; } = new();
    private MethodInfo[] Methods => TestSet?.Info.Value.ToArray();
    private Dictionary<Type, ConstructorInfo> Prototypes { get; } = new();

    internal TestSet TestSet { get; private set; }

    public string SetName => TestSet?.Info.Key.Name ?? string.Empty;
    public Type Type => TestSet?.Info.Key;
    public IReadOnlyList<string> TestMethods => TestSet?.Info.Value.Select(mi => mi.Name).ToList() ?? new List<string>();

    public static TestContext Current { get; } = lazyContext.Value;
    public bool HasPrototypes => Prototypes.Any();

    internal TestContext() { }

    internal void Clear()
    {
        TestSet = null;
        ResultsLog.Clear();
    }

    internal void Set(TestSet testSet)
    {
        TestSet = testSet;
    }

    internal void LogTestResult(string resultMessage)
    {
        ResultsLog.Enqueue(resultMessage);
    }

    public object ConstructTestTarget()
    {
        return Activator.CreateInstance(Type) ?? new();
    }
    public IEnumerator<MethodInfo> GetMethodIterator()
    {
        foreach (var method in Methods)
        {
            yield return method;
        }
    }
    public void OutputResults(Action<string> output)
    {
        while (ResultsLog.TryDequeue(out string result))
        {
            output(result);
        }
    }

    public void AddPrototypes(Dictionary<Type, ConstructorInfo> prototypeInfo)
    {
        foreach (var pi in prototypeInfo)
        {
            //  assumption: parameterless constructor
            var param = new object[] { };

            var type = pi.Key;
            var ctor = pi.Value;

            if ((Prototype)ctor.Invoke(param) != null)
            { Prototypes.Add(type, ctor); }
        }
    }

    public ConstructorInfo Get<TPrototype>() where TPrototype : Prototype
    {
        if (Prototypes.TryGetValue(typeof(TPrototype), out ConstructorInfo ctor))
        {
            return ctor;
        }

        return null;
    }
}
