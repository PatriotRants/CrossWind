
// See https://aka.ms/new-console-template for more information

using BlankCrossWindProject;

Console.WriteLine("Sample CrossWind Project");

const string APP_NAME = "CrossWind Sample";

var app = new SampleApplication(APP_NAME);
app.Controller.Initialize(new SampleViewController(app.Name));
app.Run();
