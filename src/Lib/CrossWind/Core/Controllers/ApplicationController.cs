
namespace ForgeWorks.CrossWind.Core;

public class ApplicationController : Controller, IApplicationController
{
    public event AppStartUp OnAppStartUp;

    public ApplicationController(string name) : base(name) { }

    public void Initialize(IController controller)
    {
        throw new NotImplementedException();
    }

    internal void StartUp()
    {
        OnAppStartUp?.Invoke();
    }
}
