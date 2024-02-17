
namespace ForgeWorks.CrossWind.Core;

public interface IApplicationController : IController
{
    event AppStartUp OnAppStartUp;

    void StartUp();
}
