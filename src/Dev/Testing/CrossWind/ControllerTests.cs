using MinuteMan.LabKit;
using static MinuteMan.LabKit.TestSet;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;
using ForgeWorks.CrossWind.Presentation;

namespace CrossWind.UnitTests;

public class ControllerTests
{
    private static readonly IRegistry<IController> Controllers = Registries.Controllers;

    private IApplicationController Controller { get; }

    /// <summary>
    /// Use constructor to initialize class resources
    /// </summary>
    public ControllerTests()
    {
        Log($"Initializing Test Harness: {nameof(ControllerTests)}");
        Vassert.AreNotEqual(
            Controller = Controllers.Create<ApplicationController>("TestAppCtrlr"),
            null);
    }

    /*
        Expectation:    Construct a new registered view controller.
        Functions:  
                        - instantiate view controller
                        - registers view controller
        Test:           Registry contains controller
        Assumptions:    ?
    */
    [TestCase]
    public void CreateViewController()
    {
        const string vcName = "TEST";
        _ = new TestViewController(vcName);

        Assert.IsTrue(Controllers.TryGet(c => c.Type == typeof(TestViewController), out IController item));

        CleanUp();

        // Controller.Initialize(new TestViewController(vcName));
    }
    /*
        Expectation:    ViewController raises initialization event
        Function:       Raising event signals that the controller's `OnInitialize` method is 
                        about to be called.
        Test:           Target IsInitialized flag is TRUE
        Assumptions:    ?
    */
    [TestCase]
    public void InitializeViewController()
    {
        const string vcName = "TEST";

        IViewController viewController = new TestViewController(vcName);
        viewController.OnInitializing += (id) =>
        {
            Log($"[{nameof(InitializeViewController)}] :: Initializing ...");
        };

        viewController.Initialize();

        Assert.IsTrue(viewController.IsInitialized);

        CleanUp();
    }
    /*
        Expectation:    ViewController has a View object interface accessible
        Function:       On initialization --
                        - set View configuration
                        - ?
        Test:           Target IView has parameters initialized
        Assumptions:    ?
    */
    [TestCase]
    public void ViewControllerHasView()
    {
        const string vcName = "TEST";
        Vector2i vSize = new(1200, 900);
        WindowState vState = WindowState.Maximized;

        IViewController viewController = new TestViewController(vSize, vState, vcName);
        viewController.OnInitializing += (id) =>
        {
            Log($"[{nameof(InitializeViewController)}] :: Initializing ...");
        };

        viewController.Initialize();

        Vassert.AreNotEqual(viewController.View, default);

        var view = viewController.View;
        Vassert.AreEqual(view.Name, vcName);
        Vassert.AreEqual(view.Title, $"Window [{vcName}]");
        Vassert.AreEqual(view.Size, vSize);
        Vassert.AreEqual(view.WindowState, vState);
    }


    /*
        Expectation: ApplicationController will initialize an IController.
        Functions:  
                    - add IController to its collection (to be managed/disposed/etc)
                    - call IController.Initialize
        Test:       ApplicationController has an IWindowController
        Discussion: ApplicationController does not know what kind of resources the subsequent 
                    IController requires. For instance, IViewController will require an 
                    IWindowController. It will request an IWindowController from the 
                    ApplicationController.
    */

    //  TODO: [RE: MinuteMan] Implement CleanUp attribute
    private static void CleanUp()
    {
        Log($"\n[CleanUp] <{nameof(ControllerTests)}> Resources =======================");

        // remove controllers from registry
        var count = Controllers.GetAll<TestViewController>().Count();
        Controllers.Kill<TestViewController>();
        var remain = Controllers.GetAll<TestViewController>().Count();

        Assert.IsTrue(remain == 0 && count > remain);
        Log($"[CleanUp] Killed {count} <{nameof(TestViewController)}>");
    }


    /// <summary>
    /// Test View Controller class
    /// </summary>
    private class TestViewController : ViewController
    {
        private Vector2i WindowSize { get; } = new(800, 600);
        private WindowState WindowState { get; } = WindowState.Normal;

        public TestViewController(string name) : base(name) { }
        public TestViewController(Vector2i size, WindowState windowState, string name) : base(name)
        {

        }

        protected override void OnInitialize(View view)
        {
            //  Set View parameters here
            view.Size = WindowSize;
            view.WindowState = WindowState;
        }
    }
}

