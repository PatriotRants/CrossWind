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
    public event Func<IClient> Load;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event Action Unload;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Title => Window.Title;

    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<WindowPositionEventArgs> Move { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<ResizeEventArgs> Resize { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<FramebufferResizeEventArgs> FramebufferResize { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action Refresh { get; set; } = () => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<CancelEventArgs> Closing { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<MinimizedEventArgs> Minimized { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<MaximizedEventArgs> Maximized { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<JoystickEventArgs> JoystickConnected { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<FocusedChangedEventArgs> FocusedChanged { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<KeyboardKeyEventArgs> KeyDown { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<KeyboardKeyEventArgs> KeyUp { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<TextInputEventArgs> TextInput { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action MouseLeave { get; set; } = () => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action MouseEnter { get; set; } = () => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<MouseButtonEventArgs> MouseDown { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<MouseButtonEventArgs> MouseUp { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<MouseMoveEventArgs> MouseMove { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<MouseWheelEventArgs> MouseWheel { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<FileDropEventArgs> FileDrop { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<FrameEventArgs> UpdateFrame { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action<FrameEventArgs> RenderFrame { get; set; } = (a) => { };
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public Action SwapBuffers => Window.SwapBuffers;
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public double UpdateTime => Window.UpdateTime;
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public double UpdateFrequency
    // {
    //     get => Window.UpdateFrequency;
    //     set => Window.UpdateFrequency = value;
    // }
    // /// <summary>
    // /// <inheritdoc />
    // /// </summary>
    // public int ExpectedSchedulerPeriod
    // {
    //     get => Window.ExpectedSchedulerPeriod;
    //     set => Window.ExpectedSchedulerPeriod = value;
    // }

    // public Color4 Background { get; set; }
    // public Vector2i ClientSize { get; set; }

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

        Window.Load += OnLoad;
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
        Window?.Run();
    }

    private void OnLoad()
    {
        //  if no subscriber or null returned, use the default client
        var client = Load?.Invoke() ?? new DefaultClient();
        Window.SetClient(client);
    }
}
