using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Presentation;

namespace CrossWind.Demo;

public class BasicDemoApplication : Application
{
    private WindowController windowController { get; }

    public BasicDemoApplication(string name) : base(name)
    {
        //  WindowController adds itself to Registries
        windowController = new WindowController("Demo", name, width: 1200, height: 900, state: WindowState.Maximized);
    }

    protected override void OnStartUp()
    {
        base.OnStartUp();
        windowController.Run();
    }
}