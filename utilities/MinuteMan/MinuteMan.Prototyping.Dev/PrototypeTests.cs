using MinuteMan.LabKit;
using static MinuteMan.LabKit.TestSet;
using static MinuteMan.LabKit.Prototype;

namespace MinuteMan.Prototyping.Dev;

public class PrototypeTests
{

    TestContext Context => TestContext.Current;

    [TestCase]
    public void HasTargetPrototype()
    {
        Log($"[{nameof(PrototypeTests)}.{nameof(HasTargetPrototype)}] checking loaded prototypes ...");
        Assert.IsTrue(Context.HasPrototypes);
        Assert.IsTrue(Context.Get<TargetClass>() != null);
    }

    [TestCase]
    public void TargetHasName()
    {
        const string expected = "Ima Test";

        //  every call to Evaluate produces a new 'clean' prototype
        Evaluate<TargetClass>((cls) =>
        {
            Assert.AreEqual(cls.Name, expected);
        }, "", "");
    }
}
