using System.ComponentModel;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ForgeWorks.CrossWind.Components;

public interface IClient
{
    IGLFWGraphicsContext Context { get; set; }
    string Title { get; set; }
    Vector2i ClientSize { get; set; }
    WindowState WindowState { get; set; }

    void OnRenderView(FrameEventArgs args);
    void OnUpdateView(FrameEventArgs args);
    void OnClosingWindow(CancelEventArgs args);
    void OnFileDrop(FileDropEventArgs args);
    void OnFramebufferResize(FramebufferResizeEventArgs args);
    void OnKeyDown(KeyboardKeyEventArgs args);
    void OnKeyUp(KeyboardKeyEventArgs args);
    void OnMouseDown(MouseButtonEventArgs args);
    void OnMouseEnterWindow();
    void OnMouseLeaveWindow();
    void OnMouseWheel(MouseWheelEventArgs args);
    void OnRefresh();
    void OnResizeWindow(ResizeEventArgs args);
    void OnTextInput(TextInputEventArgs args);
    void OnWindowMaximized(MaximizedEventArgs args);
    void OnWindowMinimized(MinimizedEventArgs args);
    void OnWindowMove(WindowPositionEventArgs args);
}
