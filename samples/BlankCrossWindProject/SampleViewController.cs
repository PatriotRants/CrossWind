using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Presentation;

namespace BlankCrossWindProject;

#nullable disable
/// <summary>
/// Test View Controller class
/// </summary>
public class SampleViewController : ViewController
{
    private Vector2i Size { get; } = new(800, 600);
    private WindowState State { get; } = WindowState.Normal;

    public IApplicationController AppController { get; set; }

    public SampleViewController(string name) : this(new(1200, 900), WindowState.Maximized, name) { }
    private SampleViewController(Vector2i windowSize, WindowState windowState, string name) : base(name)
    {
        Size = windowSize;
        State = windowState;
    }

    protected override void OnInitialize(IApplicationController applicationController, View view)
    {
        //  subscribe to application controller OnRun
        (AppController = applicationController).OnAppRun += OnRun;

        //  Set View parameters here
        view.Size = Size;
        view.WindowState = State;
        view.Background = new(15, 15, 15, 0);

        AppController.Initialize(WindowController = new WindowController(view));
    }

    private void OnRun(DateTime startTime)
    {
        WindowController.Run();
    }
}


