using System.Diagnostics;
using System.Reflection;

using MinuteMan.Logging;

namespace MinuteMan.LabKit;

#nullable disable
public class Agent
{
    private TestSetCollection TestSets { get; init; }
    private TestContext Context { get; } = TestContext.Current;

    private Logger Logger { get; } = Logger.GetLogger;

    public Agent(Dictionary<Type, List<MethodInfo>> testSets)
    {
        TestSets = new(testSets);
    }

    public void Configure()
    {
        //  instantiate an instance of the TestSet (type)

        //  configure global test parameters

    }

    public void Start()
    {
        var iterator = TestSets.GetTestSetIterator();
        while (iterator.MoveNext())
        {
            Context.Set(iterator.Current);

            var setName = Context.SetName;
            var testMethods = Context.GetMethodIterator();

            if (Context.TestMethods.Any())
            {
                Logger.WriteLine($"Discovered {setName}: [{string.Join("|", Context.TestMethods)}]");
                object target = Context.ConstructTestTarget();

                Logger.WriteLine($"== Begin == [ {setName} ] ===================================");
                Stopwatch sw1, sw2;

                sw1 = InitializeStopwatch();
                sw2 = InitializeStopwatch();

                sw1.Start();
                while (testMethods.MoveNext())
                {
                    MethodInfo tm = testMethods.Current;

                    try
                    {
                        Logger.Write($"{"":15} == [ == {tm.Name:-35} == ] {"":15}");
                        var elapsed = ExecuteTest(sw2, () => tm.Invoke(target, (object[])null));
                        Logger.WriteLine($"\t {elapsed.Milliseconds}ms");
                        sw1.Restart();
                    }
                    catch (System.Exception ex)
                    {
                        Logger.WriteLine($"{ex}");
                    }

                    Context.OutputResults(Console.WriteLine);
                }
                sw1.Stop();

                Logger.WriteLine($"===  End  =========================================== {sw1.ElapsedMilliseconds:5}ms");
                Context.Clear();
            }
        }
    }

    private TimeSpan ExecuteTest(Stopwatch sw, Action method)
    {
        sw.Start();
        method();
        sw.Stop();

        return sw.Elapsed;
    }

    private static Stopwatch InitializeStopwatch()
    {
        Stopwatch sw = new();
        sw.Start();
        sw.Restart();

        return sw;
    }
}
