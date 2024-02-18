using MinuteMan.LabKit;
using static MinuteMan.LabKit.TestSet;

using ForgeWorks.CrossWind;
using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;


namespace CrossWind.UnitTests;

public class RegistriesTests
{
    /// <summary>
    /// Checks to see if all the required registries are initialized
    /// </summary>
    [TestCase]
    public void HasRegistres()
    {
        Assert.IsTrue(Registries.Controllers != null);
        Assert.IsTrue(Registries.Types != null);
    }

    /// <summary>
    /// Checks TypeRegistry has default types
    /// </summary>
    [TestCase]
    public void ConfigureTypeRegistry()
    {
        List<Type> defaultTypes = new() {
            typeof(ApplicationController),
            typeof(string),
            typeof(int)
        };

        foreach (var controllerType in defaultTypes)
        {
            if (controllerType == typeof(ApplicationController))
            {
                Assert.IsTrue(Registries.Types.Contains(controllerType));
            }
            else if (controllerType == typeof(string))
            {
                Assert.IsFalse(Registries.Types.Contains(controllerType));
            }
            else
            {
                Except.Log($"Unhandled Controller Type : {controllerType.Name}");
            }
        }
    }
    /// <summary>
    /// Ensures a duplicate type is not added to the TypeRegistry
    /// </summary>
    [TestCase]
    public void DuplicateTypeNotAdded()
    {
        TypeId expected = TypeId.Empty;
        TypeId actual = Registries.Types.Add(typeof(ApplicationController));

        Assert.AreEqual(expected, actual);
    }
    /*
        Expectation:    Retrieves all items of a specified type
        Functions:      Returns enumerable collection of items specified by type
        Test:           
                        - count: expected number of items returned
                        - verify: correct items returned
        Assumptions:    Only specified items exist in current registry
    */
    [TestCase]
    public void GetAllOfType()
    {
        const string acTEST1 = "TEST1";
        const string acTEST2 = "TEST2";

        Registries.Controllers.Create<ApplicationController>(acTEST1);
        Registries.Controllers.Create<ApplicationController>(acTEST2);

        var controllers = Registries.Controllers.GetAll<ApplicationController>()
            .ToArray();

        Assert.IsTrue(controllers.Length == 2);
        Vassert.AreEqual(acTEST1, controllers[0].Name);
        Vassert.AreEqual(acTEST2, controllers[1].Name);
    }
    /*
        Expectation:    Dispose and remove controllers from registry
        Functions:
                        - retrieves controller by type
                        - calls controller `Dispose(...)` method
                        - removes controller from registry
        Test:           Controller not in registry
        Discussion:     Until we mock controller object, we will use a log statement to visually
                        verify the Dispose method is being called.
                        Implemented `OnDisposing` event on base Controller.
                        Implemented virtual `Dispose(...)` method on Controller.
        Asseumptions:   ?
    */
    [TestCase]
    public void KillAllControllerType()
    {
        const string acTEST1 = "TEST1";
        const string acTEST2 = "TEST2";

        var ctrllrA = default(IApplicationController);
        var ctrllrB = default(IApplicationController);

        var isDisposedA = false;
        var isDisposedB = false;

        var controllers = Registries.Controllers.GetAll<ApplicationController>()
            .ToList();

        if (controllers.Count == 0)
        {
            controllers.Add(ctrllrA = Registries.Controllers.Create<ApplicationController>(acTEST1));
            controllers.Add(ctrllrB = Registries.Controllers.Create<ApplicationController>(acTEST2));
        }
        else
        {
            Assert.IsTrue(controllers.Count == 2);

            ctrllrA = (IApplicationController)controllers[0];
            ctrllrB = (IApplicationController)controllers[1];
        }

        ctrllrA.OnDisposing += (id) =>
        {
            Log($"Disposing {id.Name}");
            isDisposedA = true;
        };
        ctrllrB.OnDisposing += (id) =>
        {
            Log($"Disposing {id.Name}");
            isDisposedB = true;
        };

        var areDead = Registries.Controllers.Kill<ApplicationController>();

        Assert.IsTrue(isDisposedA);
        Assert.IsTrue(isDisposedB);
        Assert.IsTrue(areDead);
    }

    [TestCase]
    public void CreateController()
    {
        Registries.Controllers.Create<ApplicationController>("TestController");

        bool hasController = Registries.Controllers.TryGet(c => c.Name == "TestController", out IController controller);
        Assert.IsTrue(controller is ApplicationController);
    }
    [TestCase]
    public void AddCustomApplicationController()
    {
        bool isAdded = Registries.Controllers.Add(new TestAppController());
        Assert.IsTrue(isAdded);

        if (Registries.Controllers.TryGet(r => r.Name == "TestApp", out IController controller))
        {
            Log($"Retrieved Controller [{controller.Name}]");
        }
    }
}

public class TestAppController : Controller
{
    public TestAppController() : base("TestApp")
    {
    }
}
