
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace ForgeWorks.CrossWind.Presentation;

public interface IView
{
    /// <summary>
    /// Get the <see cref="View"/>'s Name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Get the <see cref="View"/>'s Title
    /// </summary>
    string Title { get; }
    /// <summary>
    /// Get the <see cref="View"/>'s WindowState
    /// </summary>
    WindowState WindowState { get; }
    /// <summary>
    /// Get the <see cref="View"/>'s Visibility
    /// </summary>
    Visibility Visibility { get; }
    /// <summary>
    /// Get the <see cref="View"/>'s Background
    /// </summary>
    Color4 Background { get; }
    /// <summary>
    /// Get the <see cref="View"/>'s ClientBounds
    /// </summary>
    Box2i ClientBounds { get; }
    /// <summary>
    /// Get the <see cref="View"/>'s Size
    /// </summary>
    Vector2i Size { get; }
    /// <summary>
    /// Get the <see cref="View"/>'s Location
    /// </summary>
    Vector2i Location { get; }
}
