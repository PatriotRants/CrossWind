
using ForgeWorks.CrossWind.Presentation;

namespace ForgeWorks.CrossWind.Core;

public class ApplicationController : Controller, IApplicationController
{
    private List<IController> Controllers { get; } = new();
    private DateTime Start { get; set; }

    public event AppRun OnAppRun;
    public event AppStartUp OnAppStartUp;
    public event OnControllerInitialized OnControllerInitialized;


    public ApplicationController(string name) : base(name) { }

    public void Initialize(IController controller)
    {
        //  make sure we don't already have this controller
        if (Controllers.Any(c => c.Handle == controller.Handle))
        {
            //  duplicate controller

            return;
        }

        //  a little smart logic :/     ... ???
        if (controller is IViewController viewController)
        {
            viewController.Initialize(this);
        }
        else
        {
            //  if we want to, we can for sanity's sake make sure the controller is in the registry
            //  I'm not choosing to do that here but keep this in mind if controllers aren't found
            controller.Initialize();
        }

        //  after we initialize, add to collection for cleanup later
        Controllers.Add(controller);

        //  finally, raise initialized event
        OnControllerInitialized?.Invoke(controller.GetIdentifier());
    }

    public void Run()
    {
        OnAppRun?.Invoke(Start = DateTime.UtcNow);
    }

    internal void StartUp()
    {
        OnAppStartUp?.Invoke();
    }
}
