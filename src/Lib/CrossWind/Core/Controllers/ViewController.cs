using OpenTK.Windowing.Common;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;
using OpenTK.Mathematics;

namespace ForgeWorks.CrossWind.Presentation;

public abstract class ViewController : Controller, IViewController
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event OnViewControllerInitializing OnInitializing;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public bool IsInitialized { get; private set; }

    /// <summary>
    /// Get or set current <see cref="ViewController"/>'s View
    /// </summary>
    protected View View { get; set; }
    protected IWindowController WindowController { get; set; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    IView IViewController.View => View;

    /// <summary>
    /// Instantiates a new registered view controller 
    /// </summary>
    /// <param name="name">ViewController name</param>
    protected ViewController(string name) : base(name)
    {
        Registries.Controllers.Add(this);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Initialize(IApplicationController controller)
    {
        OnInitializing?.Invoke(Id);
        OnInitialize(controller, View = new(Name) { Controller = this });

        IsInitialized = true;
    }

    /// <summary>
    /// When implemented in a <see cref="ViewController"/> descendent, configure 
    /// <see cref="View"/> parameters and additional resources here.
    /// </summary>
    /// <param name="view"><see cref="ViewController"/>'s View</param>
    protected virtual void OnInitialize(IApplicationController controller, View view) { /* optional */ }

    /// <summary>
    /// [OPTIONAL]
    /// </summary>
    public virtual void OnWindowLoaded() { /* optional */ }
    /// <summary>
    /// [OPTIONAL]
    /// </summary>
    public virtual void OnUpdateFrame(FrameEventArgs args) { /* optional */ }
    /// <summary>
    /// [OPTIONAL]
    /// </summary>
    public virtual void OnRenderFrame(FrameEventArgs args) { /* optional */ }

    public virtual void WindowResized(int x, int y) { /* optional */ }

    public virtual void Update(float deltaSeconds) { /* optional */ }

    public virtual void MouseScroll(Vector2 offset) { /* optional */ }

    public virtual void PressChar(char unicode) { /* optional */ }
}
