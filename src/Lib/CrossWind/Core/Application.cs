using ForgeWorks.CrossWind.Collections;
using Microsoft.Win32;

namespace ForgeWorks.CrossWind.Core;

public abstract class Application : IApplication
{
    private readonly ApplicationController appController;

    public ApplicationId Id { get; }

    public string Name => Id.Name;
    public Version CrossWindVersion { get; } = Version.Parse("0.2.24.002");
    public IApplicationController Controller => appController;

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

    public void Run()
    {
        OnStartUp();
        appController.Run();
    }
    public virtual void Dispose() { /* optional */ }

    protected virtual void OnStartUp()
    {
        appController.StartUp();
    }
}
