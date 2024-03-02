using System.ComponentModel;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ForgeWorks.CrossWind.Components;

public class DefaultClient : IClient
{
    public Color4 Background { get; set; }
    public IGLFWGraphicsContext Context { get; set; }
    public Vector2i ClientSize { get; set; }

    public void OnRenderView(FrameEventArgs args)
    {
        Console.WriteLine("Client.OnRender");

        // Show that we can use OpenGL: Clear the window to cornflower blue.
        GL.ClearColor(Background);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Show in the window the results of the rendering calls.
        Context.SwapBuffers();
    }
    public void OnUpdateView(FrameEventArgs args)
    {

    }
    public void OnClosingWindow(CancelEventArgs args)
    {

    }
    public void OnFileDrop(FileDropEventArgs args)
    {

    }
    public void OnFramebufferResize(FramebufferResizeEventArgs args)
    {

    }
    public void OnKeyDown(KeyboardKeyEventArgs args)
    {

    }
    public void OnKeyUp(KeyboardKeyEventArgs args)
    {

    }
    public void OnMouseDown(MouseButtonEventArgs args)
    {

    }
    public void OnMouseEnterWindow()
    {

    }
    public void OnMouseLeaveWindow()
    {

    }
    public void OnMouseWheel(MouseWheelEventArgs args)
    {

    }
    public void OnRefresh()
    {

    }
    public void OnResizeWindow(ResizeEventArgs args)
    {
        Console.WriteLine("Client.OnResize");

        // Update the opengl viewport
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    }
    public void OnTextInput(TextInputEventArgs args)
    {

    }
    public void OnWindowMaximized(MaximizedEventArgs args)
    {

    }
    public void OnWindowMinimized(MinimizedEventArgs args)
    {

    }
    public void OnWindowMove(WindowPositionEventArgs args)
    {

    }
}
