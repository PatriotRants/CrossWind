using OpenTK.Windowing.GraphicsLibraryFramework;

using ForgeWorks.CrossWind.Core;

namespace ForgeWorks.CrossWind.Components;

public interface IWindowController : IController
{
    //
    // Summary:
    //     Occurs before Window.Run(...)
    event Func<IClient> RunClient;
    //
    // Summary:
    //     Occurs before the window is displayed for the first time.
    event Action Load;
    //
    // Summary:
    //     Occurs before the window is destroyed.
    event Action Unload;

    /// <summary>
    /// Get the Window Title
    /// </summary>
    public string Title { get; }

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
    /// <summary>
    /// Run the WindowController execution
    /// </summary>
    void Run();
    void UpdateClient(IClient client);
}
