using System.Reflection;
using MinuteMan.Runtime;
using MinuteMan.Logging;
using MinuteMan.LabKit;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Welcome to MinuteMan: the Minimum Unit Test Manager");

AgentState agentState = AgentState.Initialize;
RunStatus status = RunStatus.Okay;

while (agentState != AgentState.Exit)
{
    switch (agentState)
    {
        case AgentState.Initialize:
            status = Initialize(ref agentState, args);

            break;
        case AgentState.Begin:
            status = Begin(ref agentState, args);

            break;
        case AgentState.Configure:
            status = Configure(ref agentState);

            break;
        case AgentState.Execute:
            status = Run(ref agentState);

            break;
        case AgentState.CleanUp:
            status = CleanUp(ref agentState);

            break;
        case AgentState.End:
        default:
            status = End(ref agentState);

            //LogStream.Close();
            Console.SetOut(DefaultOutput);
            break;
    }

    if (status == RunStatus.Error)
    {
        agentState = AgentState.Exit;
    }
}

Console.WriteLine($"MinuteMan TestAgent has exited with code {(int)status}[{status}:{ErrCode}].");

#region execution states
const string SOURCE = "-s";
const string ASSEMBLY = "-a";
const string OUTPUT = "-l";

static RunStatus Initialize(ref AgentState agentState, string[] args)
{
    RunStatus status = RunStatus.Okay;

    //  args input length just need not be zero
    if (args.Length > 0)
    {
        var iterator = args.AsReadOnly()
                           .GetEnumerator();
        while (iterator.MoveNext())
        {
            string[] param = iterator.Current.Split(":", StringSplitOptions.TrimEntries);

            switch (param[0])
            {
                case SOURCE:
                    //  we are going to just try and add - 2nd -s will not override previous values
                    Parameters.TryAdd(Args.Source, param[1]);

                    break;
                case ASSEMBLY:
                    //  user specified assembly name
                    Parameters.TryAdd(Args.Assembly, param[1]);

                    break;
                case OUTPUT:
                    //  same; do not overwrite a previous value
                    Parameters.TryAdd(Args.Output, param[1]);

                    break;
                default:
                    break;
            }
        }

        //  if we don't have Args.Source then we need to stop
        if (!ValidateSource())
        {
            agentState = AgentState.End;

            return RunStatus.Error;
        }

        Stream stream;
        //  checks to ensure we configure a console output stream (TextWriter)
        if (TrySetParameter(Args.Output, "Default"))
        {
            stream = Console.OpenStandardOutput();
        }
        else
        {
            //  now that we have a validated source, we can get the working 
            //      directory and append the correct test case log directory
            stream = InitDefaultLogging();
        }

        ConfigureLogger(stream);
        agentState = AgentState.Begin;
    }

    return status;
}

static RunStatus Begin(ref AgentState agentState, string[] args)
{
    RunStatus status = RunStatus.Okay; ;

    Logger.WriteLine("Begin MUTM Discovery ...");

    // if(args.Length < 1) {
    //     Console.WriteLine("Invalid Target Parameter");
    //     agentState = AgentState.End;

    //     return RunStatus.Error;
    // }

    // Console.WriteLine($"Root: {Directory.GetCurrentDirectory()}");
    // agentState = AgentState.End;

    // if(!ValidateSource(args[0])) {
    //     status = RunStatus.Error;
    // }
    // Console.WriteLine($"Working: {Working}");
    // Console.WriteLine($"Target: {Target}");

    if (!TryLoadAssembly(out Assembly assembly))
    {
        status = RunStatus.Error;
    }
    Logger.WriteLine($"Assembly Loaded: {assembly.GetName().Name}");
    agentState = AgentState.Configure;

    return status;
}

static RunStatus Configure(ref AgentState agentState)
{
    Logger.WriteLine("Configure Test Agent ...");

    Agent = new(TestCases);
    Agent.Configure();

    agentState = AgentState.Execute;

    return RunStatus.Okay;
}

static RunStatus Run(ref AgentState agentState)
{
    Logger.WriteLine("Execute Agent Test Runner ...");

    Agent.Start();

    agentState = AgentState.CleanUp;

    return RunStatus.Okay;
}

static RunStatus CleanUp(ref AgentState agentState)
{
    Logger.WriteLine("Clean Up Test Agent ...");
    agentState = AgentState.End;

    return RunStatus.Okay;
}

static RunStatus End(ref AgentState agentState)
{
    Logger.WriteLine("End MUTM");
    agentState = AgentState.Exit;

    return RunStatus.Okay;
}
#endregion

#nullable disable
public static partial class Program
{
    private const string TESTLOGDIR = "TestCaseLogs";

    static Dictionary<Args, string> Parameters { get; } = new();
    static TextWriter DefaultOutput { get; set; } = Console.Out;
    static Logger Logger { get; set; }
    static DirectoryInfo Working { get; set; } = new("null");
    static DirectoryInfo TestLogs { get; set; } = new("null");
    static FileInfo Target { get; set; } = new("null");
    static Dictionary<Type, List<MethodInfo>> TestCases { get; } = new();
    static int ErrCode { get; set; } = 0;
    static Agent Agent { get; set; } = null;
    /// <summary>
    /// Valide target directory contains the expected .dll
    /// </summary>
    static bool ValidateSource(string target)
    {
        //  target should be a directory to the project containing test cases
        if (!Directory.Exists(target))
        {
            ErrCode = 1001;

            return false;
        }

        //  make sure we have an assembly in the directory - 
        //      we're going to assume (for now) that the directory name is the name of the .dll to test
        Working = new(target);

        string assemblyFile = string.Empty;
        if (Parameters.TryGetValue(Args.Assembly, out string asmName))
        {
            assemblyFile = $"{asmName}.dll";
        }
        else
        {
            assemblyFile = $"{Working.Name}.dll";
        }

        if (!TryFindFile(Working, assemblyFile, out FileInfo targetFile))
        {
            ErrCode = 1002;

            // return false;
        }

        //  if target file is null, we did not find the file
        return (Target = targetFile) != null;
    }
    static bool ValidateSource()
    {
        if (!Parameters.TryGetValue(Args.Source, out string target))
        {
            Logger.WriteLine("Invalid Target Parameter");

            return false;
        }

        return ValidateSource(target);
    }
    /// <summary>
    /// Initializes the test case logging consumer
    /// </summary>
    static Stream InitDefaultLogging()
    {
        // //  assumption: Working directory has been initized if necessary
        // if(TestLogs.Name == "null") {
        //     //  nothing to change, should be default console text writer
        //     return;
        // }

        //  construct test case logging directory
        string testLogDir = Path.Combine(Working.FullName, TESTLOGDIR);
        if (!Directory.Exists(testLogDir))
        {
            Directory.CreateDirectory(testLogDir);
        }

        TestLogs = new(testLogDir);
        //  construct file path
        string testLogFile = Path.Combine(TestLogs.FullName, $"{DateTime.Now:MMddyyyyHHmm}.log");

        return new FileStream($"{testLogFile}", FileMode.Create);
    }
    /// <summary>
    /// Tries to find a file with standardized search method
    /// </summary>
    static bool TryFindFile(DirectoryInfo working, string assemblyFile, out FileInfo fileInfo)
    {
        return (fileInfo = new FileInfo(FindFile(Working, assemblyFile) ?? string.Empty)).Exists;
    }
    /// <summary>
    /// Recursive method to locate file including search through subdirectories
    /// </summary>
    static string FindFile(DirectoryInfo directory, string assemblyFile)
    {
        FileInfo file = null;

        //  1. check if assembly file exists in this directory
        if (directory.GetFiles().Where(f =>
        {
            if (f.Name.Equals(assemblyFile))
            {
                file = f;
            }

            return file != null;
        }).Any())
        {
            return file?.FullName;
        }

        //  2. check for directories
        IEnumerator<DirectoryInfo> subDirectories = directory.GetDirectories().AsEnumerable<DirectoryInfo>().GetEnumerator();
        while (subDirectories.MoveNext() && file == null)
        {
            string filePath = FindFile(new(subDirectories.Current.FullName), assemblyFile);

            file = filePath != null ? new(filePath) : file;
        }

        return file?.FullName;
    }
    /// <summary>
    /// Attempts to load an assembly
    /// </summary>
    static bool TryLoadAssembly(out Assembly assembly)
    {
        Logger.WriteLine("Loading Assembly ...");

        return HasTestCases(assembly = Assembly.LoadFrom(Target.FullName));
    }
    /// <summary>
    /// Attempts to set an initialization parameter
    /// </summary>
    static bool TrySetParameter(Args argType, string parameter)
    {
        return Parameters.TryAdd(argType, parameter);
    }
    /// <summary>
    /// Determines if the test set container has methods (Test Cases)
    /// </summary>
    static bool HasTestCases(Assembly assembly)
    {
        //  look for test cases - methods decorated with [TestCase]
        var types = assembly.GetTypes();
        List<MethodInfo> methods;

        foreach (var t in types)
        {
            methods = new();

            foreach (var m in t.GetMethods().Where(y => y.GetCustomAttributes().OfType<TestCaseAttribute>().Any()))
            {
                methods.Add(m);
            }

            if (methods.Count > 0)
            {
                TestCases.Add(t, methods.ToList());
            }
        }

        return TestCases
            .Where(kv => types.Any(t => t.Equals(kv.Key)))
            .Any();
    }
    /// <summary>
    /// Configure Logging In/Out channels
    /// </summary>
    static void ConfigureLogger(Stream outputLogStream)
    {
        //  save the standard output.
        DefaultOutput = Console.Out;
        Logger = Logger.Configure(outputLogStream);
    }
}