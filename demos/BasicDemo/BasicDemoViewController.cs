using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Presentation;

namespace CrossWind.Demo;

/// <summary>
/// ViewController class
/// </summary>
public class BasicDemoViewController : ViewController
{

    public IApplicationController AppController { get; set; }

    public BasicDemoViewController(string name) : this(new(1200, 900), WindowState.Maximized, name) { }
    private BasicDemoViewController(Vector2i windowSize, WindowState windowState, string name) : base(name)
    {
        Title = Name;
        Size = windowSize;
        State = windowState;
    }

    protected override void OnInitialize(IApplicationController applicationController)
    {
        //  subscribe to application controller OnRun
        (AppController = applicationController).OnAppRun += OnRun;

        //  Set View parameters here
        State = WindowState.Maximized;
        Background = new(15, 15, 15, 0);

        AppController.Initialize(WindowController = new WindowController(this));
    }

    private void OnRun(DateTime startTime)
    {
        WindowController.Run();
    }
}