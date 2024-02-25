using System.ComponentModel;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace ForgeWorks.CrossWind.Components;

public interface IWindow
{
    // Summary:
    //     Gets a double representing the time spent in the UpdateFrame function, in seconds.
    public double UpdateTime { get; }
    // Summary:
    //     Gets or sets a double representing the update frequency, in hertz.
    //
    // Remarks:
    //     A value of 0.0 indicates that UpdateFrame events are generated at the maximum
    //     possible frequency (i.e. only limited by the hardware's capabilities).
    //
    //     Values lower than 1.0Hz are clamped to 0.0. Values higher than 500.0Hz are clamped
    //     to 500.0Hz.
    public double UpdateFrequency { get; set; }
    // Summary:
    //     The expected scheduler period in milliseconds. Used to provide accurate sleep
    //     timings.
    //
    //     On Windows the scheduler period can be set using timeBeginPeriod(), OpenTK sets
    //     this value to 8ms by default. See OpenTK.Windowing.Desktop.GameWindow.Run for
    //     more details.
    //     On Linux we set this to 1 as it seems like System.Threading.Thread.Sleep(System.Int32)
    //     is able to accurately sleep 1ms.
    //     On macos we set this to 1 aswell as tests imply System.Threading.Thread.Sleep(System.Int32)
    //     can accurately sleep 1ms.
    public int ExpectedSchedulerPeriod { get; set; }
    //
    // Summary:
    //     Occurs before the window is displayed for the first time.
    event Action Load;
    //
    // Summary:
    //     Occurs before the window is destroyed.
    event Action Unload;
    /// <summary>
    /// Occurs whenever the window is moved.
    /// </summary>
    public event Action<WindowPositionEventArgs> Move;
    /// <summary>
    /// Occurs whenever the window is resized.
    /// </summary>
    public event Action<ResizeEventArgs> Resize;
    /// <summary>
    /// Occurs whenever the framebuffer is resized.
    /// </summary>
    public event Action<FramebufferResizeEventArgs> FramebufferResize;
    /// <summary>
    /// Occurs whenever the window is refreshed.
    /// </summary>
    public event Action Refresh;
    /// <summary>
    /// Occurs when the window is about to close.
    /// </summary>
    public event Action<CancelEventArgs> Closing;
    /// <summary>
    /// Occurs when the window is minimized.
    /// </summary>
    public event Action<MinimizedEventArgs> Minimized;
    /// <summary>
    /// Occurs when the window is maximized.
    /// </summary>
    public event Action<MaximizedEventArgs> Maximized;
    /// <summary>
    /// Occurs when a joystick is connected or disconnected.
    /// </summary>
    public event Action<JoystickEventArgs> JoystickConnected;
    /// <summary>
    /// Occurs when the <see cref="NativeWindow.IsFocused" /> property of the window changes.
    /// </summary>
    public event Action<FocusedChangedEventArgs> FocusedChanged;
    /// <summary>
    /// Occurs whenever a keyboard key is pressed.
    /// </summary>
    public event Action<KeyboardKeyEventArgs> KeyDown;
    /// <summary>
    /// Occurs whenever a Unicode code point is typed.
    /// </summary>
    public event Action<TextInputEventArgs> TextInput;
    /// <summary>
    /// Occurs whenever a keyboard key is released.
    /// </summary>
    public event Action<KeyboardKeyEventArgs> KeyUp;
    /// <summary>
    /// Occurs whenever the mouse cursor leaves the window <see cref="NativeWindow.Bounds" />.
    /// </summary>
    // FIXME: This this when we leave the client rectangle or the window bounds?
    public event Action MouseLeave;
    /// <summary>
    /// Occurs whenever the mouse cursor enters the window <see cref="NativeWindow.Bounds" />.
    /// </summary>
    // FIXME: This this when we enter the client rectangle or the window bounds?
    public event Action MouseEnter;
    /// <summary>
    /// Occurs whenever a <see cref="MouseButton" /> is clicked.
    /// </summary>
    public event Action<MouseButtonEventArgs> MouseDown;
    /// <summary>
    /// Occurs whenever a <see cref="MouseButton" /> is released.
    /// </summary>
    public event Action<MouseButtonEventArgs> MouseUp;
    /// <summary>
    /// Occurs whenever the mouse cursor is moved;
    /// </summary>
    public event Action<MouseMoveEventArgs> MouseMove;
    /// <summary>
    /// Occurs whenever a mouse wheel is moved;
    /// </summary>
    public event Action<MouseWheelEventArgs> MouseWheel;
    /// <summary>
    /// Occurs whenever one or more files are dropped on the window.
    /// </summary>
    public event Action<FileDropEventArgs> FileDrop;

    Color4 Background { get; }

    //
    // Summary:
    //     Initialize the update thread (if using a multi-threaded context, and enter the
    //     game loop of the GameWindow).
    //
    // Remarks:
    //     On windows this function sets the thread affinity mask to 0x0001 to avoid the
    //     thread from changing cores.
    //
    //     On windows this function calls timeBeginPeriod(8) to get better sleep timings,
    //     which can increase power usage. This can be undone by calling timeEndPeriod(8)
    //     in OpenTK.Windowing.Desktop.GameWindow.OnLoad and timeBeginPeriod(8) in OpenTK.Windowing.Desktop.GameWindow.OnUnload.
    //     If the expected scheduler time is changed set OpenTK.Windowing.Desktop.GameWindow.ExpectedSchedulerPeriod
    //     to the appropriate value to keep the accuracy of the update loop.
    unsafe void Run();
    //
    // Summary:
    //     Swaps the front and back buffers of the current GraphicsContext, presenting the
    //     rendered scene to the user.
    void SwapBuffers();
    void Close();
    void Dispose();
}