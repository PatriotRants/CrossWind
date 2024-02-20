using static MinuteMan.LabKit.TestSet;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Presentation;

namespace CrossWind.Prototype;

#nullable disable
/// <summary>
/// Test View Controller class
/// </summary>
public class TestViewController : ViewController
{
    public IApplicationController AppController { get; set; }

    public TestViewController(string name) : base(name) { }
    public TestViewController(Vector2i windowSize, WindowState windowState, string name) : this(name)
    {
        Size = windowSize;
        State = windowState;
        Title = name;
    }

    protected override void OnInitialize(IApplicationController applicationController)
    {
        //  subscribe to application controller OnRun
        (AppController = applicationController).OnAppRun += OnRun;

        //  Set View parameters here
        Background = new(15, 15, 15, 0);

        AppController.Initialize(WindowController = new WindowController(this));
    }

    private void OnRun(DateTime startTime)
    {
        Log($"[{Name}.{nameof(OnRun)}] Start: {startTime}");
        WindowController.Run();
    }
}


