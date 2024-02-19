using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using ForgeWorks.CrossWind.Presentation;

namespace ForgeWorks.CrossWind.Components;

internal class Window
{
    private const string DEFAULT_NAME = "window_";

    private static byte winCount = 1;

    private readonly InternalWindow window;
    private WindowState previousState { get; set; } = WindowState.Normal;

    internal nint Context => window.Context.WindowPtr;

    internal MouseState MouseState => window.MouseState;
    internal KeyboardState KeyboardState => window.KeyboardState;

    public string Name
    {
        get;
        init;
    }
    public bool IsAlive
    {
        get;
        set;
    }
    public string Title
    {
        get => window.Title;
        set => window.Title = value;
    }
    public Visibility Visibility
    {
        get => GetVisibility();
        set => SetVisibility(value);
    }
    public Color4 Background
    {
        get => window.Background;
        set => SetBackground(value);
    }
    public Box2i ClientArea
    {
        get => window.ClientRectangle;
    }
    public WindowState State
    {
        get => window.WindowState;
        set => SetWindowState(value);
    }
    public Vector2i Size
    {
        get => window.Size;
        set => SetWindowSize(value);
    }
    public Vector2i Location
    {
        get => window.Location;
    }
    public IGraphicsContext GraphicsContext => window.Context;

    public Window(int width = 800, int height = 600, string title = "MainWindow", string name = "")
    {
        //  handle default name
        if (string.IsNullOrEmpty(name))
        {
            name = CreateDefaultName();
        }

        window = new InternalWindow(width, height, title);
        Name = name;

        // should be unnecesary
        // Width = width;
        // Height = height;
        // Title = window.Title;

        //  sub events - for internal use only
        window.UpdateFrame += OnUpdateFrame;
        window.RenderFrame += OnRenderFrame;
        window.Activated += OnWindowCreated;
        window.KeyDown += OnKeyPressed;
        window.KeyUp += OnKeyReleased;
        window.TextInput += OnTextInput;
        window.Resize += OnWindowResized;
        window.Move += OnWindowMoved;
        window.MouseWheel += OnMouseWheel;
    }

    private static string CreateDefaultName()
    {
        return $"{DEFAULT_NAME}{winCount++:000}";
    }


    private void OnMouseWheel(MouseWheelEventArgs args)
    {
        //guiController.MouseScroll(args.Offset);
    }
    private void OnKeyReleased(KeyboardKeyEventArgs args)
    {
        // released = args.Key;
        // pressed &= ~args.Key;
    }
    private void OnKeyPressed(KeyboardKeyEventArgs args)
    {
        //pressed |= args.Key;
        //keyBindings.TryExecute(args.Key);
    }
    private void OnWindowResized(ResizeEventArgs args)
    {
        // OnResized?.Invoke(new()
        // {
        //     Event = WindowEvent.Resized,
        //     Name = () => Name,
        //     Size = () => Size,
        //     Client = () => ClientArea
        // });
    }
    private void OnWindowMoved(WindowPositionEventArgs args)
    {
        // OnMoved?.Invoke(new()
        // {
        //     Event = WindowEvent.Moved,
        //     Name = () => Name,
        //     Location = () => Location
        // });
    }
    private void OnTextInput(TextInputEventArgs args)
    {
        // guiController.PressChar((char)args.Unicode);
    }
    public void Run()
    {
        // appContext = applicationContext;
        // keyBindings = applicationContext.KeyBindings;

        IsAlive = true;
        window.Run();
    }
    public void ToggleFullscreen()
    {
        switch (State)
        {
            case WindowState.Normal:
            case WindowState.Maximized:
            case WindowState.Minimized:
                SetWindowState(WindowState.Fullscreen);

                break;
            case WindowState.Fullscreen:
                SetWindowState(WindowState.Normal);

                break;
            default:
                break;
        }
    }

    protected virtual void OnUpdateFrame(FrameEventArgs args)
    {
        // guiController.Update((float)args.Time);
        window.WindowDirty = true;
    }
    protected virtual void OnRenderFrame(FrameEventArgs args)
    {
        if (window.WindowDirty)
        {
            //  redraw window
            GL.ClearColor(Background);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            window.WindowDirty = false;

            // guiController.Render();

            window.SwapBuffers();
        }
    }
    protected virtual void OnActivated() { /* optional */ }
    protected void Dispose(bool disposing)
    {
        //  unsub events
        window.UpdateFrame -= OnUpdateFrame;
        window.Close();
        //  dispose native window
        window.Dispose();

        IsAlive = false;
    }

    private void OnWindowCreated()
    {
        //  now push Loaded Event into Pipeline
        // IEvent.EventPipe.Send<WindowChangedEvent>(new()
        // {
        //     Event = WindowEvent.Created,
        //     Name = () => Name,
        //     Title = () => Title,
        //     Size = () => Size,
        //     Client = () => ClientArea
        // });

        //  call event
        OnActivated();
    }
    private void SetWindowSize(Vector2i size)
    {
        window.Size = size;
        window.WindowDirty = true;
    }
    private Visibility GetVisibility()
    {
        return window.IsVisible ? Visibility.Visible : Visibility.Hidden;
    }
    private void SetVisibility(Visibility visibility)
    {
        window.IsVisible = visibility == Visibility.Visible;

        // OnStateChanged?.Invoke(new()
        // {
        //     Event = WindowEvent.Visibility,
        //     Name = () => Name,
        //     Visibility = () => Visibility,
        //     State = () => State
        // });
    }
    private void SetBackground(Color4 color)
    {
        window.Background = (Color)color;
        window.WindowDirty = true;
    }
    private void SetWindowState(WindowState windowState)
    {
        previousState = window.WindowState;

        window.WindowState = windowState;
        window.WindowDirty = true;

        // OnStateChanged?.Invoke(new()
        // {
        //     Event = WindowEvent.StateChange,
        //     Name = () => Name,
        //     Visibility = () => Visibility,
        //     State = () => State
        // });
    }

    private class InternalWindow : GameWindow
    {
        internal bool WindowDirty { get; set; }
        internal Color Background { get; set; } = Color.Transparent;

        internal event Action Activated;

        internal InternalWindow() : this(800, 600, nameof(InternalWindow)) { }
        internal InternalWindow(int width, int height, string title) : this(GameWindowSettings.Default, new NativeWindowSettings()
        {
            ClientSize = (width, height),
            Title = title
        })
        { }
        private InternalWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            WindowDirty = true;
        }

        protected override void OnLoad()
        {
            Activated?.Invoke();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            WindowDirty = true;
        }
        protected override void OnMaximized(MaximizedEventArgs e)
        {
            base.OnMaximized(e);

            WindowDirty = true;
        }
        protected override void OnMinimized(MinimizedEventArgs e)
        {
            base.OnMinimized(e);

            WindowDirty = true;
        }
    }
}
