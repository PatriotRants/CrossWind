
// See https://aka.ms/new-console-template for more information

using CrossWind.Gui;

Console.WriteLine("Sample CrossWind GUI ");

const string APP_NAME = "CrossWind GUI Sample";

var app = new GuiApplication(APP_NAME);
app.Controller.Initialize(new GuiViewController(app.Name));
app.Run();
