**CrossWind**

*CrossWind* is an extensible library wrapping OpenTK developed to run on Linux and Windows Operating Systems.  

The purpose is to explore a different approach to design and development of *Window Views*. Taking the idea of a Windows Forms development paradigm - Model-View-Presenter (MVP) - I have tucked the `OpenTK.GameWindow` under the hood. The `WindowController` has sole responsibility of the actual `Window` object. In a simple project, an execution occurs without a console and instantiates an `Application` object. The application is managed by an `ApplicationController`. When start up is initiated, the developer instantiates the `WindowController` with some window parameters. The controller then instantiates the actual `Window`. When `Application.Run(...)` is executed, `WindowController` calls the underlying `GameWindow.Run(..)`.  

`WindowController` exposes all the events and properties of the underlying `GameWindow`. See the Basic Demo for an example.

**`Collections.Registries`** Namespace
*`Controllers`* - ApplicationController, WindowController, etc. - are all mantained in a Controller Registry.
*`Types`* - A type registry to maintain a unique identifier for each registered type.

Developers can create their own registries with the `IRegistry<TRegistry>` interface or implement a concrete class from `Registry<T, TInterface>`.

**Utilities**
As of now, there are 2 utilities included in this repository. Eventually, they will likely take up residence in their spaces. They were just useful here while I got CrossWind off the ground.  

First, **`MinuteMan`**. It is a *Minimal UnitTest Manager*. It is extremely lightweight but very functional. The scope of test functions is very sparse mostly because I only implemented the `Assert` methods I most often use: `IsNull`, `IsNotNull`, `IsTrue`, `IsFalse`, `AreEqual`, `AreNotEqual`. The *equality* assertions have a `Vassert` compliment that provides a more verbose logging of the test results.

Second, **`CrossWind Project Creator`**. From a JSON configuration, a CrossWind project is created in a given directory. There is a lot more to do with this but I've only imlemented the very basics as a tool for my projects.
