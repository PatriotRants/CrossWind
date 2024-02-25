using ForgeWorks.CrossWind.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ForgeWorks.CrossWind.Presentation;

public interface IWindowController : IController
{
    /// <summary>
    /// Run the WindowController execution
    /// </summary>
    void Run();

    /// <summary>
    /// Get the Window KeyboardState
    /// </summary>
    /// <returns></returns>
    KeyboardState GetKeyboardState();
    /// <summary>
    /// Get the Window MouseState
    /// </summary>
    /// <returns></returns>
    MouseState GetMouseState();
}
