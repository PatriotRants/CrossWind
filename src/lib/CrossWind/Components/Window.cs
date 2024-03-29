using System.ComponentModel;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using ForgeWorks.CrossWind.Presentation;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ForgeWorks.CrossWind.Components;

public abstract class Window
{
    private GameWindow _window;
    private IClient _client;

    /// <summary>
    /// Get the current <see cref="WindowController"/>
    /// </summary>
    internal WindowController Controller { get; init; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action Load { get; set; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action Unload { get; set; } = () => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<WindowPositionEventArgs> Move { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<ResizeEventArgs> Resize { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<FramebufferResizeEventArgs> FramebufferResize { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action Refresh { get; set; } = () => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<CancelEventArgs> Closing { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<MinimizedEventArgs> Minimized { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<MaximizedEventArgs> Maximized { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<JoystickEventArgs> JoystickConnected { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<FocusedChangedEventArgs> FocusedChanged { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<KeyboardKeyEventArgs> KeyDown { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<KeyboardKeyEventArgs> KeyUp { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<TextInputEventArgs> TextInput { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action MouseLeave { get; set; } = () => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action MouseEnter { get; set; } = () => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<MouseButtonEventArgs> MouseDown { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<MouseButtonEventArgs> MouseUp { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<MouseMoveEventArgs> MouseMove { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<MouseWheelEventArgs> MouseWheel { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<FileDropEventArgs> FileDrop { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<FrameEventArgs> UpdateFrame { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action<FrameEventArgs> RenderFrame { get; set; } = (a) => { };
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Action SwapBuffers => _window.SwapBuffers;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public double UpdateTime => _window.UpdateTime;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public double UpdateFrequency
    {
        get => _window.UpdateFrequency;
        set => _window.UpdateFrequency = value;
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public int ExpectedSchedulerPeriod
    {
        get => _window.ExpectedSchedulerPeriod;
        set => _window.ExpectedSchedulerPeriod = value;
    }
    public WindowState WindowState
    {
        get;
        set;
    }
    /// <summary>
    /// Get or set the background color
    /// </summary>
    public Color4 Background
    {
        get;
        set;
    }
    public string Name { get; }
    public string Title { get; }
    public Vector2i ClientSize { get; }
    public IClient Client => _client;

    internal Window() : this((800, 600), nameof(Window), "Window") { }
    internal Window(Vector2i size, string title, string name)
    {
        ClientSize = size;
        Name = name;
        Title = title;
    }

    public void Run()
    {
        _window = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Title = Title,
            ClientSize = ClientSize,
            WindowState = WindowState
        });

        _window.Load += OnLoad;
        _window.Unload += OnUnload;
        _window.JoystickConnected += JoystickConnected;
        _window.FocusedChanged += FocusedChanged;

        //  this will raise OnLoad(..)
        _window.Run();
    }


    private void OnLoad()
    {
        Load?.Invoke();

        _window.UpdateFrame += Client.OnUpdateView;
        _window.RenderFrame += Client.OnRenderView;
        _window.Resize += Client.OnResizeWindow;
        _window.Move += Client.OnWindowMove;
        _window.FramebufferResize += Client.OnFramebufferResize;
        _window.Refresh += Client.OnRefresh;
        _window.Closing += Client.OnClosingWindow;
        _window.Maximized += Client.OnWindowMaximized;
        _window.Minimized += Client.OnWindowMinimized;
        _window.KeyDown += Client.OnKeyDown;
        _window.KeyUp += Client.OnKeyUp;
        _window.TextInput += Client.OnTextInput;
        _window.MouseLeave += Client.OnMouseLeaveWindow;
        _window.MouseEnter += Client.OnMouseEnterWindow;
        _window.MouseDown += Client.OnMouseDown;
        _window.MouseWheel += Client.OnMouseWheel;
        _window.FileDrop += Client.OnFileDrop;
    }
    private void OnUnload()
    {
        _window.UpdateFrame -= Client.OnUpdateView;
        _window.RenderFrame -= Client.OnRenderView;
        _window.Resize -= Client.OnResizeWindow;
        _window.Move -= Client.OnWindowMove;
        _window.FramebufferResize -= Client.OnFramebufferResize;
        _window.Refresh -= Client.OnRefresh;
        _window.Closing -= Client.OnClosingWindow;
        _window.Maximized -= Client.OnWindowMaximized;
        _window.Minimized -= Client.OnWindowMinimized;
        _window.KeyDown -= Client.OnKeyDown;
        _window.KeyUp -= Client.OnKeyUp;
        _window.TextInput -= Client.OnTextInput;
        _window.MouseLeave -= Client.OnMouseLeaveWindow;
        _window.MouseEnter -= Client.OnMouseEnterWindow;
        _window.MouseDown -= Client.OnMouseDown;
        _window.MouseWheel -= Client.OnMouseWheel;
        _window.FileDrop -= Client.OnFileDrop;

        Unload?.Invoke();
    }

    protected virtual void OnRenderFrame(FrameEventArgs args)
    {

    }

    protected virtual void OnResize(ResizeEventArgs e)
    {

    }

    internal KeyboardState GetKeyboardState()
    {
        return _window?.KeyboardState;
    }

    internal MouseState GetMouseState()
    {
        return _window?.MouseState;
    }

    internal void SetClient(IClient client)
    {
        (_client = client).Context = _window.Context;
    }
}
