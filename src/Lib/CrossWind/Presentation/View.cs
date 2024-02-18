using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace ForgeWorks.CrossWind.Presentation;

public class View : IView
{
    /// <summary>
    /// Get or set current <see cref="View"/> Title
    /// </summary>
    public string Title
    {
        get;
        set;
    }
    /// <summary>
    /// Get current <see cref="View"/> Name
    /// </summary>
    public string Name
    {
        get;
    }
    /// <summary>
    /// Get or set current <see cref="View"/> WindowState
    /// </summary>
    public WindowState WindowState
    {
        get;
        set;
    } = WindowState.Normal;
    /// <summary>
    /// Get or set current <see cref="View"/> Visibility
    /// </summary>
    public Visibility Visibility
    {
        get;
        set;
    } = Visibility.Hidden;
    /// <summary>
    /// Get or set current <see cref="View"/> Background
    /// </summary>
    public Color4 Background
    {
        get;
        set;
    }
    /// <summary>
    /// Get current <see cref="View"/> ClientBounds
    /// </summary>
    public Box2i ClientBounds
    {
        get;
    }
    /// <summary>
    /// Get or set current <see cref="View"/> Size
    /// </summary>
    public Vector2i Size
    {
        get;
        set;
    }
    /// <summary>
    /// Get or set current <see cref="View"/> Location
    /// </summary>
    public Vector2i Location
    {
        get;
        set;
    }

    internal View(string name)
    {
        Name = name;
        Title = $"Window [{Name}]";
    }
}
