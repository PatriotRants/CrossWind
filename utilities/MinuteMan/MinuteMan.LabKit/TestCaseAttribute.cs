namespace MinuteMan.LabKit;

/// <summary>
/// Identifies a test case (unit test method). When a unit test is identified, the parent class is marked as a test harness.
/// If there are configurations or clean up to be executed, use [TestSetup] and [TestCleanUp].
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestCaseAttribute : Attribute
{

    public TestCaseAttribute() { }
}
