using System.Reflection;

using MinuteMan.LabKit;
using static MinuteMan.LabKit.TestSet;

using CrossWind.Prototype.Harness;
using static CrossWind.Prototype.Harness.Handlers;
using static CrossWind.Prototype.Harness.Evaluator;

using ForgeWorks.CrossWind.Core;


const string KEYBIND_CONFIG = "keybinding.cfg";
File.Delete(KEYBIND_CONFIG);

const bool SKIP = false;
const bool EVAL = true;

string n = string.Empty;
string d = string.Empty;

Log("Welcome to Crosswind Prototyping");
Log("=========================================================\n");

/* skip evaluations with flag */
//  run no-op application
n = "No-Op";
d = "run no-op application";
Evaluate<NoOpApplication>(SKIP, (app) => app.Run(), n, d);

//  run no-op application with application controller events
n = "No-Op App Controller";
d = "run no-op application with application controller events";
Evaluate<NoOpApplication>(EVAL, (app) =>
{
    //  get the application controller
    IApplicationController controller = app.Controller;
    Vassert.AreNotEqual(controller, default);

    controller.OnAppStartUp += () => OnNoOpAppStartUp(n);
    app.Run();
}, n, d);


namespace CrossWind.Prototype.Harness
{
    internal class Evaluator
    {
        private static TestContext Context { get; } = TestContext.Current;

        public static bool Evaluate<TApp>(bool flag, Action<TApp> evaluate, string name, string description) where TApp : Application
        {
            if (!flag)
            {
                Log($"[Eval] Skip Evaluation: <{typeof(TApp).Name}>");
                Log("=========================================================\n");

                return flag;
            }
            else
            {
                Log($"[Eval] Evaluate: <{typeof(TApp).Name}>");
            }

            Evaluate(evaluate, name, description);

            Log("=========================================================\n");

            Context.OutputResults(Console.WriteLine);

            return flag;
        }

        private static void Evaluate<TApp>(Action<TApp> evaluate, string name, string description) where TApp : Application
        {
            //  construct application
            TApp app = Construct<TApp>(name, description, typeof(TApp));

            //  evaluate
            evaluate(app);
            //  cleanup
            app.Dispose();
        }

        private static TApp Construct<TApp>(string name, string description, Type appType) where TApp : Application
        {
            var ctor = appType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, new Type[] { typeof(string) });

            Log($"Description: {description}");

            TApp app = (TApp)ctor.Invoke(new object[] { name });
            return app;
        }
    }

    internal class NoOpApplication : Application
    {
        public NoOpApplication(string name) : base(name) { }

        protected override void OnStartUp()
        {   //  DOC: if overriden, either call `base.OnStartUp` or `Controller.StartUp`
            Log($"[App: {Name}] (v.{CrossWindVersion})::{Id.GetHashCode()}");
            Controller.StartUp();
        }
    }

    internal class Handlers
    {
        internal static void OnNoOpAppStartUp(string name)
        {
            Log($"[{nameof(Handlers)}.{nameof(OnNoOpAppStartUp)}] {name}");
        }
    }

    #region test windows

    #endregion
}