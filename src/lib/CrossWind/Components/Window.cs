using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

using ForgeWorks.CrossWind.Presentation;

namespace ForgeWorks.CrossWind.Components;

internal class Window : GameWindow, IWindow
{
    /// <summary>
    /// Determines if the Window is dirty - has changes, resized, etc.
    /// </summary>
    internal bool IsDirty { get; set; }
    /// <summary>
    /// Get the current <see cref="WindowController"/>
    /// </summary>
    internal WindowController Controller { get; init; }
    /// <summary>
    /// Get or set the background color
    /// </summary>
    public Color4 Background { get; internal set; }

    internal Window() : this((800, 600), nameof(Window), "Window") { }
    internal Window(Vector2i size, string title, string name) : this(GameWindowSettings.Default, new NativeWindowSettings()
    {
        Title = title,
        ClientSize = size,
    })
    { }
    private Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        IsDirty = true;
    }
}
