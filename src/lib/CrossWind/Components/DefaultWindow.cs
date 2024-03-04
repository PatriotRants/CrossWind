using OpenTK.Mathematics;

namespace ForgeWorks.CrossWind.Components;

public class DefaultWindow : Window
{
    public DefaultWindow() : this("Window", "Window")
    {
    }

    public DefaultWindow(string title, string name) : base(title, name)
    {
    }
}
