using OpenTK.Windowing.GraphicsLibraryFramework;

using ForgeWorks.CrossWind.Core;

namespace ForgeWorks.CrossWind.Components;

public interface IWindowController : IController
{
    //
    // Summary:
    //     Occurs before the window is displayed for the first time.
    event Func<IClient> Load;
    //
    // Summary:
    //     Occurs before the window is destroyed.
    event Action Unload;
    /*     /// <summary>
        /// Occurs whenever the window is moved.
        /// </summary>
        Action<WindowPositionEventArgs> Move { get; set; }
        /// <summary>
        /// Occurs whenever the window is resized.
        /// </summary>
        Action<ResizeEventArgs> Resize { get; set; }
        /// <summary>
        /// Occurs whenever the framebuffer is resized.
        /// </summary>
        Action<FramebufferResizeEventArgs> FramebufferResize { get; set; }
        /// <summary>
        /// Occurs whenever the window is refreshed.
        /// </summary>
        Action Refresh { get; set; }
        /// <summary>
        /// Occurs when the window is about to close.
        /// </summary>
        Action<CancelEventArgs> Closing { get; set; }
        /// <summary>
        /// Occurs when the window is minimized.
        /// </summary>
        Action<MinimizedEventArgs> Minimized { get; set; }
        /// <summary>
        /// Occurs when the window is maximized.
        /// </summary>
        Action<MaximizedEventArgs> Maximized { get; set; }
        /// <summary>
        /// Occurs when a joystick is connected or disconnected.
        /// </summary>
        Action<JoystickEventArgs> JoystickConnected { get; set; }
        /// <summary>
        /// Occurs when the <see cref="NativeWindow.IsFocused" /> property of the window changes.
        /// </summary>
        Action<FocusedChangedEventArgs> FocusedChanged { get; set; }
        /// <summary>
        /// Occurs whenever a keyboard key is pressed.
        /// </summary>
        Action<KeyboardKeyEventArgs> KeyDown { get; set; }
        /// <summary>
        /// Occurs whenever a keyboard key is released.
        /// </summary>
        Action<KeyboardKeyEventArgs> KeyUp { get; set; }
        /// <summary>
        /// Occurs whenever a Unicode code point is typed.
        /// </summary>
        Action<TextInputEventArgs> TextInput { get; set; }
        /// <summary>
        /// Occurs whenever the mouse cursor leaves the window <see cref="NativeWindow.Bounds" />.
        /// </summary>
        // FIXME: This this when we leave the client rectangle or the window bounds?
        Action MouseLeave { get; set; }
        /// <summary>
        /// Occurs whenever the mouse cursor enters the window <see cref="NativeWindow.Bounds" />.
        /// </summary>
        // FIXME: This this when we enter the client rectangle or the window bounds?
        Action MouseEnter { get; set; }
        /// <summary>
        /// Occurs whenever a <see cref="MouseButton" /> is clicked.
        /// </summary>
        Action<MouseButtonEventArgs> MouseDown { get; set; }
        /// <summary>
        /// Occurs whenever a <see cref="MouseButton" /> is released.
        /// </summary>
        Action<MouseButtonEventArgs> MouseUp { get; set; }
        /// <summary>
        /// Occurs whenever the mouse cursor is moved;
        /// </summary>
        Action<MouseMoveEventArgs> MouseMove { get; set; }
        /// <summary>
        /// Occurs whenever a mouse wheel is moved;
        /// </summary>
        Action<MouseWheelEventArgs> MouseWheel { get; set; }
        /// <summary>
        /// Occurs whenever one or more files are dropped on the window.
        /// </summary>
        Action<FileDropEventArgs> FileDrop { get; set; }
        /// <summary>
        /// Occurs when it is time to update a frame.
        /// </summary>
        Action<FrameEventArgs> UpdateFrame { get; set; }
        /// <summary>
        /// Occurs when it is time to render a frame.
        /// </summary>
        Action<FrameEventArgs> RenderFrame { get; set; }
        /// <summary>
        /// <inheritdoc cref="GameWindow.SwapBuffers"/>
        /// </summary>
        public Action SwapBuffers { get; }

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
        int ExpectedSchedulerPeriod { get; set; }

        /// <summary>
        /// Get the window background color
        /// </summary>
        Color4 Background { get; set; }
        /// <summary>
        /// Get the window client size
        /// </summary>
        Vector2i ClientSize { get; set; } */

    /// <summary>
    /// Get the Window KeyboardState
    /// </summary>
    /// <returns></returns>
    KeyboardState GetKeyboardState();
    /// <summary>
    /// Get the Window MouseState
    /// </summary>
    /// <returns></returns>
    MouseState GetMouseState();
    /// <summary>
    /// Run the WindowController execution
    /// </summary>
    void Run();
}
