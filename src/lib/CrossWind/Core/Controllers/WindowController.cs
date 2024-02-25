using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;
using Window = ForgeWorks.CrossWind.Components.Window;

namespace ForgeWorks.CrossWind.Presentation;

/// <summary>
/// Publically accesible Window Controller
/// </summary>
public class WindowController : Controller, IWindowController
{
    private Window Window { get; }

    /// <summary>
    /// Constructs a new WindowController object. Automatically registers itself in the Controllers Registry.
    /// </summary>
    public WindowController(string name, string title, int width = 800, int height = 900, WindowState state = WindowState.Maximized) : base(name)
    {
        Registries.Controllers.Add(this);

        Window = new((width, height), title, name)
        {
            WindowState = state,
            Background = Color.FromArgb(0, 15, 15, 15)
        };

        Window.UpdateFrame += OnUpdateFrame;
        Window.RenderFrame += OnRenderFrame;
        Window.Resize += OnWindowResize;
        Window.TextInput += OnTextInput;
        Window.MouseDown += OnMouseDown;
        Window.MouseWheel += OnMouseWheel;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public KeyboardState GetKeyboardState()
    {
        return Window.KeyboardState;
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public MouseState GetMouseState()
    {
        return Window.MouseState;
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void Run()
    {
        Window?.Run();
    }

    /// <summary>
    /// <inheritdoc cref="GameWindow.OnUpdateFrame"/>
    /// </summary>
    /// <param name="args"></param>
    protected virtual void OnUpdateFrame(FrameEventArgs args)
    {
        Window.IsDirty = true;
    }
    /// <summary>
    /// <inheritdoc cref="GameWindow.OnRenderFrame"/>
    /// </summary>
    /// <param name="args"></param>
    protected virtual void OnRenderFrame(FrameEventArgs args)
    {
        if (Window.IsDirty)
        {
            // Show that we can use OpenGL: Clear the window to cornflower blue.
            GL.ClearColor(Window.Background);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Show in the window the results of the rendering calls.
            Window.SwapBuffers();
            Window.IsDirty = false;
        }
    }
    /// <summary>
    /// Called when window is resized
    /// </summary>
    /// <param name="args"></param>
    protected virtual void OnWindowResize(ResizeEventArgs args)
    {
        // Update the opengl viewport
        GL.Viewport(0, 0, Window.ClientSize.X, Window.ClientSize.Y);
    }
    /// <summary>
    /// Called when window receives text input
    /// </summary>
    /// <param name="args"></param>
    protected virtual void OnTextInput(TextInputEventArgs args)
    {

    }
    /// <summary>
    /// Called when a mouse button is pressed
    /// </summary>
    /// <param name="args"></param>
    protected virtual void OnMouseDown(MouseButtonEventArgs args)
    {

    }
    /// <summary>
    /// Called when the mouse wheel is scrolled
    /// </summary>
    /// <param name="args"></param>
    protected virtual void OnMouseWheel(MouseWheelEventArgs args)
    {

    }
}
