using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using ForgeWorks.CrossWind.Presentation;

namespace ForgeWorks.CrossWind.Components;

internal class Window : GameWindow
{
    internal bool IsDirty { get; set; }
    internal Color4 Background { get; set; } = Color.LightGray;

    internal event Action OnWindowLoaded;
    internal WindowController Controller { get; init; }

    internal Window() : this((800, 600), nameof(Window), "Window") { }
    internal Window(Vector2i size, string title, string name) : this(GameWindowSettings.Default, new NativeWindowSettings()
    {
        ClientSize = size,
        Title = title
    })
    { }
    private Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        IsDirty = true;
    }

    protected override void OnLoad()
    {
        OnWindowLoaded?.Invoke();
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

        IsDirty = true;
    }
    protected override void OnMaximized(MaximizedEventArgs e)
    {
        base.OnMaximized(e);

        IsDirty = true;
    }
    protected override void OnMinimized(MinimizedEventArgs e)
    {
        base.OnMinimized(e);

        IsDirty = true;
    }
}
