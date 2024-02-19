using OpenTK.Windowing.Common;

using ForgeWorks.CrossWind.Core;
using OpenTK.Mathematics;

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
    Vector2i Size { get; }
    WindowState State { get; }
    Color4 Background { get; }
    string Title { get; }

    void Initialize(IApplicationController controller);

    void Update(float deltaSeconds);

    void OnWindowLoaded();
    void WindowResized(int x, int y);
    void MouseScroll(Vector2 offset);
    void PressChar(char unicode);
}
