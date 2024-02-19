
using ForgeWorks.CrossWind.Core;

namespace ForgeWorks.CrossWind.Presentation;

public delegate void OnViewControllerInitializing(ControllerId controllerId);

public interface IViewController : IController
{
    /// <summary>
    /// Event raised when <see cref="ViewController"/> is initializing 
    /// </summary>
    event OnViewControllerInitializing OnInitializing;

    /// <summary>
    /// Determines if the <see cref="ViewController"/> has been initialized 
    /// </summary>
    bool IsInitialized { get; }
    /// <summary>
    /// Get the current <see cref="ViewController"/>'s <see cref="View"/>
    /// </summary>
    IView View { get; }

    void Initialize(IApplicationController controller);
}
