using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;
using ForgeWorks.CrossWind.Presentation;

namespace CrossWind.Demo;

public class BasicDemoApplication : Application
{
    private WindowController windowController { get; }

    public BasicDemoApplication(string name) : base(name)
    {
        //  WindowController adds itself to Registries
        windowController = new WindowController("Demo", name);
    }

    protected override void OnStartUp()
    {
        base.OnStartUp();
        windowController.Run();
    }
}