
namespace ForgeWorks.CrossWind.Core;

public delegate void AppStartUp();

public interface IApplicationController : IController
{
    event AppStartUp OnAppStartUp;

    void Initialize(IController controller);
}
