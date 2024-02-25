
namespace ForgeWorks.CrossWind.Core;

public delegate void AppRun(DateTime startTime);
public delegate void AppStartUp();
public delegate void OnControllerInitialized(IIdentifier controllerId);

/// <summary>
/// Application Controller interface
/// </summary>
public interface IApplicationController : IController
{
    /// <summary>
    /// Event raised when the Application's Run(...) method is invoked
    /// </summary>
    event AppRun OnAppRun;
    /// <summary>
    /// Event raised just prior to ApplicationController's Run(...) method being invoked
    /// </summary>
    event AppStartUp OnAppStartUp;
    /// <summary>
    /// Event raised after the current ApplicationController is initialized
    /// </summary>
    event OnControllerInitialized OnControllerInitialized;

    /// <summary>
    /// Initialize the specified controller
    /// </summary>
    /// <param name="controller">The <see cref="IController"/> to initialize</param>
    void Initialize(IController controller);
    /// <summary>
    /// Start the ApplicationController's execution
    /// </summary>
    void Run();
}
