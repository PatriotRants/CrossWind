using ForgeWorks.CrossWind.Collections;
using ForgeWorks.CrossWind.Presentation;

namespace ForgeWorks.CrossWind.Core;

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
    /// <exception cref="NotImplementedException"></exception>
    public void Initialize()
    {
        OnInitializing?.Invoke(Id);
        OnInitialize(View = new(Name));

        IsInitialized = true;
    }

    /// <summary>
    /// When implemented in a <see cref="ViewController"/> descendent, configure 
    /// <see cref="View"/> parameters and additional resources here.
    /// </summary>
    /// <param name="view"><see cref="ViewController"/>'s View</param>
    protected virtual void OnInitialize(View view) { /* optional */ }
}
