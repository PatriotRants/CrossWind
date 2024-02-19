using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;

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
}
