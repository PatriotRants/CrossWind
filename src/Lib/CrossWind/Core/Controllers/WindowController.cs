using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;
using Window = ForgeWorks.CrossWind.Components.Window;

namespace ForgeWorks.CrossWind.Presentation;

public class WindowController : Controller, IWindowController
{
    private IViewController ViewController { get; }
    private Window Window { get; }

    public WindowController(View view) : base(view.Name)
    {
        Registries.Controllers.Add(this);
        ViewController = view.Controller;

        Window = new(view.Size, view.Title, view.Name)
        {
            Controller = this,
            WindowState = view.WindowState,
            Background = view.Background
        };

        Window.Load += ViewController.OnWindowLoaded;
        Window.UpdateFrame += OnUpdateFrame;
        Window.RenderFrame += OnRenderFrame;
        Window.Resize += OnWindowResize;
        Window.TextInput += OnTextInput;
        Window.MouseWheel += OnMouseWheel;
    }

    public KeyboardState GetKeyboardState()
    {
        return Window.KeyboardState;
    }
    public MouseState GetMouseState()
    {
        return Window.MouseState;
    }
    public void Run()
    {
        Window?.Run();
    }

    protected virtual void OnUpdateFrame(FrameEventArgs args)
    {
        Window.IsDirty = true;
    }
    protected virtual void OnRenderFrame(FrameEventArgs args)
    {
        if (Window.IsDirty)
        {
            ViewController.Update((float)args.Time);
            Window.IsDirty = false;
            Window.SwapBuffers();
        }
    }
    private void OnWindowResize(ResizeEventArgs args)
    {
        // Update the opengl viewport
        GL.Viewport(0, 0, Window.ClientSize.X, Window.ClientSize.Y);

        // Tell ImGui of the new size
        ViewController.WindowResized(Window.ClientSize.X, Window.ClientSize.Y);
    }
    private void OnMouseWheel(MouseWheelEventArgs args)
    {
        ViewController.MouseScroll(args.Offset);
    }
    private void OnTextInput(TextInputEventArgs args)
    {
        ViewController.PressChar((char)args.Unicode);
    }
}
