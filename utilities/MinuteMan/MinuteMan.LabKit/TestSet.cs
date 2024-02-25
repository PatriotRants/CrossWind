using System.Reflection;

namespace MinuteMan.LabKit;

public delegate TestContext TestAssert();
public delegate TestContext TestAssertVerbose(bool isVerbose);
public delegate TestContext Exception();
public delegate void Log(string log);


#nullable disable
public abstract class TestSet
{
    internal static readonly TestContext TestContext = TestContext.Current;

    private Type Type { get; }
    private MethodInfo[] Methods { get; }

    internal KeyValuePair<Type, List<MethodInfo>> Info { get; }

    public TestContext Context { get; } = TestContext;

    // can be null if the test set container is a static class
    public static TestAssert Assert => () => TestContext;
    public static TestAssertVerbose Vassert => (f) => TestContext;
    public static Exception Except => () => TestContext;
    public static Log Log => LogEntry;


    public TestSet(KeyValuePair<Type, List<MethodInfo>> info)
    {
        Info = info;
    }

    private static void LogEntry(string log)
    {
        TestContext.LogTestResult(log);
    }
}

public static class Assertions
{
    const string PASS = "Pass";
    const string FAIL = "Fail";

    public static void IsTrue(this TestAssert assert, bool condition)
    {
        Log(assert(), nameof(IsTrue), condition ? PASS : FAIL);
    }

    public static void IsFalse(this TestAssert assert, bool condition)
    {
        Log(assert(), nameof(IsFalse), !condition ? PASS : FAIL);
    }

    public static void AreEqual<T>(this TestAssert assert, T p1, T p2, string message = "")
    {
        Log(assert(), nameof(AreEqual), string.IsNullOrEmpty(message) ? p1.Equals(p2) ? PASS : FAIL : message);
    }

    public static void AreEqual<T>(this TestAssertVerbose vassert, T p1, T p2, string message = "")
    {
        Log(vassert(true), $"{nameof(AreEqual)} :: {p1}.Equals({p2})", string.IsNullOrEmpty(message) ? p1.Equals(p2) ? PASS : FAIL : message);
    }

    public static void AreNotEqual<T>(this TestAssert assert, T p1, T p2, string message = "")
    {
        Log(assert(), nameof(AreNotEqual), string.IsNullOrEmpty(message) ? !p1.Equals(p2) ? PASS : FAIL : message);
    }

    public static void AreNotEqual<T>(this TestAssertVerbose vassert, T p1, T p2, string message = "")
    {
        Log(vassert(true), $"{nameof(AreNotEqual)} :: !{p1}.Equals({p2})", string.IsNullOrEmpty(message) ? !p1.Equals(p2) ? PASS : FAIL : message);
    }

    public static void Log(this Exception exception, string message)
    {
        Log(exception(), nameof(Log), $"Exception: {message}");
    }

    private static void Log(TestContext context, string func, string message)
    {
        context.LogTestResult($"{"":55}{func} [{message}]");
    }
}