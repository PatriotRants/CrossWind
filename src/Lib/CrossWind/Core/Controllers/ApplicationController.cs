
namespace ForgeWorks.CrossWind.Core;

public delegate void AppStartUp();

public class ApplicationController : Controller, IApplicationController
{
    public event AppStartUp OnAppStartUp;

    public ApplicationController(string name) : base(name, typeof(ApplicationController))
    {

    }

    public void StartUp()
    {
        OnAppStartUp?.Invoke();
    }
}
