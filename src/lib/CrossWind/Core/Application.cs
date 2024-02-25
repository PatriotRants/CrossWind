using ForgeWorks.CrossWind.Collections;
using Microsoft.Win32;

namespace ForgeWorks.CrossWind.Core;

public abstract class Application : IApplication
{
    private readonly ApplicationController appController;

    /// <summary>
    /// Get the current Application's <see cref="ApplicationId"/>
    /// </summary>
    public ApplicationId Id { get; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Name => Id.Name;
    /// <summary>
    /// <inheritdoc />
    /// TODO: remove hardcoding
    /// </summary>
    public Version CrossWindVersion { get; } = Version.Parse("0.2.24.002");
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public IApplicationController Controller => appController;

    /// <summary>
    /// Initialize a new Application
    /// </summary>
    /// <param name="name">The application name</param>
    protected Application(string name)
    {
        Id = new(GetType())
        {
            Name = name
        };

        if (!Registries.Controllers.TryGet(c => c.Type == typeof(ApplicationController), out IController controller))
        {
            appController = Registries.Controllers.Create<ApplicationController>(name);
        }
        else
        {
            appController = (ApplicationController)controller;
        }

    }

    /// <summary>
    /// Starts application execution
    /// </summary>
    public void Run()
    {
        OnStartUp();
        appController.Run();
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void Dispose() { /* optional */ }
    /// <summary>
    /// When overridden initialize additional resources.
    /// Call <see cref="base.OnStartUp"/> to raise the ApplicationController's StartUp(...) method
    /// </summary>
    protected virtual void OnStartUp()
    {
        appController.StartUp();
    }
}
