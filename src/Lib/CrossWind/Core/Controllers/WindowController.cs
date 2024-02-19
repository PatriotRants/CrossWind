using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;
using ForgeWorks.CrossWind.Components;

namespace ForgeWorks.CrossWind.Presentation;

public class WindowController : Controller, IWindowController
{
    private IViewController ViewController { get; }
    private Window Window { get; }

    public WindowController(View view) : base(view.Name)
    {
        Registries.Controllers.Add(this);
        ViewController = view.Controller;

        Window = new(view.Size, view.Title, view.Name)
        {
            Controller = this,
            Background = view.Background
        };
    }

    public void Run()
    {
        Window?.Run();
    }
}
