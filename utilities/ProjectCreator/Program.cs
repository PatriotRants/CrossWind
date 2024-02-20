// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;

using CrossWind.ProjectCreator;

#nullable disable
Console.WriteLine("CrossWind Project Creator");

const string CREATE = "-c";
const string SRC_PATH = "source_files";
const string JSON_FILE = "project.json";
const string PROJ_FILE = "csproj.xml";

string[] SOURCES = {
    Path.Combine(ProjectSourcePath.Value, SRC_PATH, "CrossWindProgram.source"),
    Path.Combine(ProjectSourcePath.Value, SRC_PATH, "CrossWindApplication.source"),
    Path.Combine(ProjectSourcePath.Value, SRC_PATH, "CrossWindViewController.source")
};

RunState runState = RunState.Initialize;
RunAction runAction = RunAction.None;
bool runFlag = true;

while (runState != RunState.Exit)
{
    switch (runState)
    {
        case RunState.Initialize:
            //  determine action based on any switches
            runAction = Initialize(ref runState, args);

            break;
        case RunState.Begin:
            //   read up all the files
            SourcePaths = SOURCES;
            runFlag = Begin(ref runState, runAction);

            break;
        case RunState.Configure:
            //  configure output path
            runFlag = Configure(ref runState);

            break;
        case RunState.Execute:
            //  write project file
            runFlag = WriteOutputFiles(ref runState);

            break;
        case RunState.Cleanup:
            runFlag = WriteBinaries(ref runState);

            break;
        case RunState.End:
        default:
            runState = RunState.Exit;

            break;
    }

    if (!runFlag && runState != RunState.Exit)
    {
        runState = RunState.Cleanup;
    }
}

static RunAction Initialize(ref RunState runState, string[] args)
{
    RunAction runAction = RunAction.CreateProject;

    if (args.Length > 0)
    {
        var iterator = args.AsReadOnly()
            .GetEnumerator();

        while (iterator.MoveNext())
        {
            string[] param = iterator.Current.Split(":", StringSplitOptions.TrimEntries);

            if (param[0] == CREATE)
            {
                runAction = RunAction.CreateJson;
            }

        }
    }

    runState = RunState.Begin;

    return runAction;
}

static bool Begin(ref RunState runState, RunAction runAction)
{
    Func<bool>[] actions = {
        LoadProjectConfiguration,
        LoadSourceFiles,
        LoadProjectFile
    };
    bool isOkay = true;

    switch (runAction)
    {
        case RunAction.CreateJson:
            isOkay = CreateDefaultJsonTemplate();
            runState = RunState.Exit;

            break;
        case RunAction.CreateProject:
            var iterator = actions
                .AsEnumerable()
                .GetEnumerator();

            while (isOkay && iterator.MoveNext())
            {
                isOkay &= iterator.Current();
            }

            break;
        case RunAction.None:
        default:
            isOkay = false;

            break;
    }

    runState = RunState.Configure;
    return isOkay;
}
static bool Configure(ref RunState runState)
{
    var currentDirectory = Directory.GetCurrentDirectory();

    //  requires that the directory exists
    var relative = Path.GetRelativePath(Template.CrossWind.Root, currentDirectory);
    Directory.SetCurrentDirectory(relative);
    string rootPath = Path.GetFullPath(Template.CrossWind.Root);
    bool isOkay = (Root = new(rootPath)).Exists;

    if (isOkay)
    {
        isOkay = (ProjectPath = Root.CreateSubdirectory(Template.CrossWind.Project)).Exists;
    }

    if (isOkay)
    {
        var binDir = ProjectPath.CreateSubdirectory("bin");
        var dbgDir = binDir.CreateSubdirectory("Debug");
        BinariesPath = dbgDir.CreateSubdirectory(Template.Project.PropertyGroup.TargetFramework);

        var objDir = ProjectPath.CreateSubdirectory("obj");
        dbgDir = objDir.CreateSubdirectory("Debug");
        dbgDir.CreateSubdirectory(Template.Project.PropertyGroup.TargetFramework);
    }

    Directory.SetCurrentDirectory(currentDirectory);
    runState = isOkay ? RunState.Execute : RunState.Exit;
    return isOkay;
}
static bool WriteOutputFiles(ref RunState runState)
{
    Func<bool>[] actions = {
        WriteCSProjFile,
        WriteSourceFiles
    };
    bool isOkay = true;

    var iterator = actions
        .AsEnumerable()
        .GetEnumerator();
    while (isOkay && iterator.MoveNext())
    {
        isOkay &= iterator.Current();
    }

    runState = RunState.Cleanup;
    return isOkay;
}
static bool WriteBinaries(ref RunState runState)
{
    bool isOkay = true;
    var sourceBinaries = Path.Combine(ProjectSourcePath.Value, "dlls");

    foreach (var source in Directory.GetFiles(sourceBinaries))
    {
        var dest = Path.GetFileName(source);
        File.Copy(source, Path.Combine(BinariesPath.FullName, dest), true);
    }

    runState = RunState.End;
    return isOkay;
}

static bool CreateDefaultJsonTemplate()
{
    bool isOkay = true;
    string jsonPath = Path.Combine(ProjectSourcePath.Value, JSON_FILE);

    Template template = new();
    string json = JsonSerializer.Serialize(template, typeof(Template), new JsonSerializerOptions()
    {
        WriteIndented = true
    });

    using (var file = File.Create(jsonPath))
    {
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        file.Write(bytes);
    }

    return isOkay;
}
static bool LoadProjectConfiguration()
{
    bool isOkay = true;
    Template = default;

    string jsonPath = Path.Combine(ProjectSourcePath.Value, JSON_FILE);
    byte[] bytes = File.ReadAllBytes(jsonPath);
    Template = (Template)JsonSerializer.Deserialize(bytes, typeof(Template));

    return isOkay && Template != null;
}
static bool LoadProjectFile()
{
    bool isOkay = true;

    string projPath = Path.Combine(ProjectSourcePath.Value, PROJ_FILE);
    ProjectFile = File.ReadAllText(projPath);

    return isOkay;
}
static bool LoadSourceFiles()
{
    bool isOkay = true;

    foreach (var sPath in SourcePaths)
    {
        SourceFiles.Add(sPath, File.ReadAllText(sPath));
    }

    return isOkay;
}
static bool WriteCSProjFile()
{
    bool isOkay = true;

    ProjectFile = ProjectFile.Replace("{sdk}", Template.Project.Sdk);
    ProjectFile = ProjectFile.Replace("{exe}", Template.Project.PropertyGroup.OutputType);
    ProjectFile = ProjectFile.Replace("{target}", Template.Project.PropertyGroup.TargetFramework);
    ProjectFile = ProjectFile.Replace("{ref_incl}", Template.Project.ItemGroup[0].Include);
    ProjectFile = ProjectFile.Replace("{hint}", ((DllReference)Template.Project.ItemGroup[0]).HintPath);
    ProjectFile = ProjectFile.Replace("{pkg_incl}", Template.Project.ItemGroup[1].Include);
    ProjectFile = ProjectFile.Replace("{pkg_ver}", ((PackageReference)Template.Project.ItemGroup[1]).Version);

    string csProjFile = Path.Combine(ProjectPath.FullName, $"{Template.CrossWind.Project}.csproj");
    using (var stream = File.OpenWrite(csProjFile))
    {
        stream.SetLength(0);
        byte[] bytes = Encoding.UTF8.GetBytes(ProjectFile);
        stream.Write(bytes);
    }

    return isOkay;
}
static bool WriteSourceFiles()
{
    bool isOkay = true;
    int count = 0;

    while (count < SourceFiles.Count)
    {
        string path = SourcePaths[count];
        string source = SourceFiles[path];

        //  alter files
        if (path.Contains("CrossWindProgram.source"))
        {
            source = source.Replace("{namespace}", Template.Project.PropertyGroup.RootNamespace);
            source = source.Replace("{proj_name}", Template.CrossWind.Project);
            source = source.Replace("{app_name}", Template.CrossWind.Name);
            path = Path.Combine(ProjectPath.FullName, $"{Template.CrossWind.Name}Program.cs");
        }
        else if (path.Contains("CrossWindApplication.source"))
        {
            source = source.Replace("{namespace}", Template.Project.PropertyGroup.RootNamespace);
            source = source.Replace("{app_name}", Template.CrossWind.Name);
            path = Path.Combine(ProjectPath.FullName, $"{Template.CrossWind.Name}Application.cs");
        }
        else if (path.Contains("CrossWindViewController.source"))
        {
            source = source.Replace("{namespace}", Template.Project.PropertyGroup.RootNamespace);
            source = source.Replace("{app_name}", Template.CrossWind.Name);
            path = Path.Combine(ProjectPath.FullName, $"{Template.CrossWind.Name}ViewController.cs");
        }
        else
        {
            isOkay = false;
        }

        if (isOkay)
        {
            //  write the file
            using (var stream = File.OpenWrite(path))
            {
                stream.SetLength(0);
                byte[] bytes = Encoding.UTF8.GetBytes(source);
                stream.Write(bytes);
            }
        }

        ++count;
    }

    return isOkay;
}

static partial class Program
{
    public static string GetSourceFilePathName([CallerFilePath] string? callerFilePath = null) => callerFilePath ?? "";
    public static Template Template { get; set; }
    public static string ProjectFile { get; set; }
    public static string[] SourcePaths { get; set; }
    public static Dictionary<string, string> SourceFiles { get; set; } = new();
    public static DirectoryInfo Root { get; set; }
    public static DirectoryInfo ProjectPath { get; set; }
    public static DirectoryInfo BinariesPath { get; set; }
}

#region csproj configuration template
public class Template
{
    public Properties CrossWind { get; set; } = new();
    public Project Project { get; set; } = new();
}
public class Properties
{
    public string Name { get; set; } = "{app_name}";
    public string Version { get; set; } = "0.2.24.001";
    public string Root { get; set; } = "{directory}";
    public string Project { get; set; } = "{proj_name}";
}
public class Project
{
    public string Sdk { get; set; } = "Microsoft.NET.Sdk";
    public PropertyGroup PropertyGroup { get; set; } = new();
    public List<Reference> ItemGroup { get; set; } = new()
        {
            new DllReference(),
            new PackageReference()
        };
}
public class PropertyGroup
{
    public string OutputType { get; set; } = "Exe";
    public string TargetFramework { get; set; } = "net7.0";
    public string RootNamespace { get; set; } = "{namespace}";
}

[JsonDerivedType(typeof(DllReference), typeDiscriminator: "dll")]
[JsonDerivedType(typeof(PackageReference), typeDiscriminator: "pkg")]
public class Reference
{
    public virtual string Include { get; set; } = string.Empty;
}

public class DllReference : Reference
{
    public override string Include { get; set; } = "ForgeWorks.CrossWind";
    [JsonInclude]
    public string HintPath { get; set; } = "ForgeWorks.CrossWind.dll";
}
public class PackageReference : Reference
{
    public override string Include { get; set; } = "OpenTK";
    [JsonInclude]
    public string Version { get; set; } = "4.8.2";
}
#endregion

internal static class ProjectSourcePath
{
    //  courtesy of https://stackoverflow.com/a/66285728/210709
    private const string myRelativePath = nameof(Program) + ".cs";
    private static string lazyValue;
    public static string Value => lazyValue ??= calculatePath();

    private static string calculatePath()
    {
        string pathName = Program.GetSourceFilePathName();
        return pathName.Substring(0, pathName.Length - myRelativePath.Length);
    }
}