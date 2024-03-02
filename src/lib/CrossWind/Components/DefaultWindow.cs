using OpenTK.Mathematics;

namespace ForgeWorks.CrossWind.Components;

public class DefaultWindow : Window
{
    public DefaultWindow() : this((800, 600), "Window", "Window")
    {
    }

    public DefaultWindow(Vector2i size, string title, string name) : base(size, title, name)
    {
    }
}
