
using ForgeWorks.CrossWind.Presentation;

namespace ForgeWorks.CrossWind.Core;

public class ApplicationController : Controller, IApplicationController
{
    private List<IController> Controllers { get; } = new();
    private DateTime Start { get; set; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event AppRun OnAppRun;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event AppStartUp OnAppStartUp;
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public event OnControllerInitialized OnControllerInitialized;

    /// <summary>
    /// Constructs a new ApplicationConroller
    /// </summary>
    /// <param name="name"></param>
    public ApplicationController(string name) : base(name) { }

    /// <summary>
    /// <inheritdoc />
    /// <para>
    /// If the controller is found in the registry, it is assumed the controller has already 
    /// been initialized. Upon initialization, the controller is added to the registry and the 
    /// <see cref="OnControllerInitialized"/> event is raised.
    /// </para>
    /// </summary>
    /// <param name="controller">The controller to initialize</param>
    public void Initialize(IController controller)
    {
        //  make sure we don't already have this controller
        if (Controllers.Any(c => c.Handle == controller.Handle))
        {
            //  duplicate controller

            return;
        }

        controller.Initialize();
        //  after we initialize, add to collection for cleanup later
        Controllers.Add(controller);
        //  finally, raise initialized event
        OnControllerInitialized?.Invoke(controller.GetIdentifier());
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void Run()
    {
        OnAppRun?.Invoke(Start = DateTime.UtcNow);
    }
    /// <summary>
    /// Call to raise the <see cref="OnAppStartUp"/> event
    /// </summary>
    internal void StartUp()
    {
        OnAppStartUp?.Invoke();
    }
}
