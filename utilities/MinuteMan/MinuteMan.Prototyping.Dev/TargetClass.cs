using MinuteMan.LabKit;

using ForgeWorks.CrossWind.Components;

namespace MinuteMan.Prototyping.Dev;

[Prototype]
public class TargetClass : Prototype<IWindow>
{

    public string Name { get; } = "Ima Test";

    // assumption: parameterless constructor
    public TargetClass()
    {
        TestSet.Log($"[{nameof(TargetClass)}] Initializing ...");
    }
}
