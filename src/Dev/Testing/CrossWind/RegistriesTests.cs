using MinuteMan.LabKit;
using static MinuteMan.LabKit.TestSet;

using ForgeWorks.CrossWind;
using ForgeWorks.CrossWind.Core;
using ForgeWorks.CrossWind.Collections;


namespace UnitTests;

public class RegistriesTests
{
    private static readonly DateTime NOW = DateTime.Now;

    DateTime Now { get; set; } = NOW;
    DateTime Then { get; set; }

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
    public TestAppController() : base("TestApp", typeof(TestAppController))
    {
    }
}
