
namespace ForgeWorks.CrossWind.Core;

public delegate void AppRun(DateTime startTime);
public delegate void AppStartUp();
public delegate void OnControllerInitialized(IIdentifier controllerId);

public interface IApplicationController : IController
{
    event AppRun OnAppRun;
    event AppStartUp OnAppStartUp;
    event OnControllerInitialized OnControllerInitialized;

    void Initialize(IController controller);
    void Run();
}
