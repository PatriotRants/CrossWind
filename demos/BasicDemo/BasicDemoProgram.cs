using CrossWind.Demo;

Console.WriteLine("CrossWind: [BasicDemo]");

const string APP_NAME = "BasicDemo";

var app = new BasicDemoApplication(APP_NAME);
app.Controller.Initialize(new BasicDemoViewController(app.Name));
app.Run();
