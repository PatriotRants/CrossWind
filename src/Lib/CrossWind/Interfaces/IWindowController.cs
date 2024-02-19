using ForgeWorks.CrossWind.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ForgeWorks.CrossWind.Presentation;

public interface IWindowController : IController
{
    void Run();

    KeyboardState GetKeyboardState();
    MouseState GetMouseState();
}
