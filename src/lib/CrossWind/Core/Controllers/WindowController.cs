using OpenTK.Windowing.Common;
using MouseState = OpenTK.Windowing.GraphicsLibraryFramework.MouseState;
using KeyboardState = OpenTK.Windowing.GraphicsLibraryFramework.KeyboardState;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Components;
using ForgeWorks.CrossWind.Collections;

namespace ForgeWorks.CrossWind.Presentation;

/// <summary>
/// Publically accesible Window Controller
/// </summary>
public sealed class WindowController : Controller, IWindowController
{
    private Window Window { get; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event Func<IClient> RunClient;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event Action Load;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event Action Unload;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Title => Window.Title;

    /// <summary>
    /// Constructs a new WindowController object. Automatically registers itself in the Controllers Registry.
    /// </summary>
    public WindowController(string name, string title, WindowState state = WindowState.Maximized) : base(name)
    {
        Registries.Controllers.Add(this);

        Window = new DefaultWindow(title, name)
        {
            WindowState = state,
            Background = new(15, 15, 15, 0),
        };

        Window.Load += Load;
        Window.Unload += Unload;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public KeyboardState GetKeyboardState()
    {
        return Window?.GetKeyboardState();
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public MouseState GetMouseState()
    {
        return Window?.GetMouseState();
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void Run()
    {
        //  if no subscriber or null returned, use the default client
        var client = RunClient?.Invoke() ?? new DefaultClient();
        Window.SetClient(client);

        Window?.Run();
    }

    public void UpdateClient(IClient client)
    {
        Window.UpdateClient(client);
    }
}
