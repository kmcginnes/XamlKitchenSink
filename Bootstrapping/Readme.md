# Lessons Learned: Bootstrapping

This weeks lesson is bootstrapping your application.

* Clear and understandable flow
* Synchronous, but not on the main thread
* Logging is always the first thing initialized
* Don't rely on magic, control your destiny

Every application requires some setup to get going. In .Net this typically involves a few things:

* Initialize the logging framework and get a logger
* Register unhandled exception handlers
* Parse app settings
* Register IoC components
* Run database migrations
* Bind communication endpoints
* Register with whatever framework we're using

## Initialize Logging

The very first thing an application should do when it is executed is initialize it's logging library. I don't care what you use (e.g. log4net, nlog, serilog, etc.) because they all work equally well. Just make logging your first priority. I like to start my logs off with an ASCII art banner of the apps name. This makes finding each application launch in the logs much simpler. I usually follow that up with the assembly file version so I always know what I'm looking at. Here's an example of what my logging initialization looks like:

```c#
private void InitializeLogging()
{
    // Set the main thread's name to make it clear in the logs.
    Thread.CurrentThread.Name = "Main";

    // Sets my logger to the console, which goes to the debug output.
    Log.InitializeWith<ConsoleLog>();

    // Show a banner to easily pick out where new instances start
    // in the log file. Plus it just looks cool.
    this.Log().Info(@" ______               __          __                                     ");
    this.Log().Info(@"|   __ \.-----.-----.|  |_.-----.|  |_.----.---.-.-----.-----.-----.----.");
    this.Log().Info(@"|   __ <|  _  |  _  ||   _|__ --||   _|   _|  _  |  _  |  _  |  -__|   _|");
    this.Log().Info(@"|______/|_____|_____||____|_____||____|__| |___._|   __|   __|_____|__|  ");
    this.Log().Info(@"                                                 |__|  |__|              ");
    this.Log().Info(@"    ");

    // Show some basic information about the assembly.
    var assemblyLocation = GetAssemblyDirectory();
    var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
    var productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
    var principal = WindowsIdentity.GetCurrent().IfNotNull(x => x.Name, "[Unknown]");

    this.Log().Info("Assembly location: {0}", assemblyLocation);
    this.Log().Info(" Assembly version: {0}", assemblyVersion);
    this.Log().Info("     File version: {0}", fileVersion);
    this.Log().Info("  Product version: {0}", productVersion);
    this.Log().Info("       Running as: {0}", principal);
}

private static string GetAssemblyDirectory()
{
    var codeBase = Assembly.GetExecutingAssembly().CodeBase;
    var uri = new UriBuilder(codeBase);
    var path = Uri.UnescapeDataString(uri.Path);
    return Path.GetDirectoryName(path);
}
```

## Unhandled Exceptions

Next, you should set up handlers for unhandled exceptions. Every framework/library has different ways of handling this, so figure out what they are and log them at the very least (this is what the FATAL log level is for). Doing this as early as possible allows you to find issues during your bootstrapping process. They happen. Trust me.

```c#
private void RegisterExceptionHandlers()
{
    // If we are running this app in Visual Studio then do not handle
    // any of the unhandled exceptions. Let Visual Studio catch them.
    if (AppDomain.CurrentDomain.FriendlyName.EndsWith("vshost.exe"))
    {
        return;
    }

    AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
    {
        var loggerTarget = sender ?? this;
        var exception = args.ExceptionObject as Exception;
        loggerTarget.Log().Fatal(exception, "Unhandled exception in the app domain.");
    };

    TaskScheduler.UnobservedTaskException += (sender, args) =>
    {
        var loggerTarget = sender ?? this;
        loggerTarget.Log().Fatal(args.Exception, "Unhandled exception in the task scheduler.");
    };

    Application.Current.DispatcherUnhandledException += (sender, args) =>
    {
        var loggerTarget = sender ?? this;
        loggerTarget.Log().Fatal(args.Exception, "Unhandled exception in the application dispatcher.");
    };
}
```

Next, we start stitching our app together with our inversion of control container. Pick your poison; they're all good. Find one that allows you to use the design patterns you want to use. For example, very few containers have first class support for generic decorators, which is an extremely powerful pattern when used correctly. Do not fall into the trap of picking the container with a lot of features outside of simply dependency resolution and controlling lifetimes. That's their only job.

