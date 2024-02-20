using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

using ForgeWorks.CrossWind.Presentation;

namespace ForgeWorks.CrossWind.Components;

internal class Window : GameWindow
{
    internal bool IsDirty { get; set; }
    internal Color4 Background { get; set; } = Color.LightGray;

    internal WindowController Controller { get; init; }

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
