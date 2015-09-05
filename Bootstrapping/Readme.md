# Lessons Learned: Bootstrapping

This weeks lesson is bootstrapping your application.

* Clear and understandable flow
* Synchronous, but not on the main thread
* Logging is always the first thing initialized
* Don't rely on magic, control your destiny

Every application requires some setup to get going. In .Net this typically involves a few things:

* Initialize the logging framework and get a logger
* Initialize the metrics recording framework
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

This results in output similar to below depending on your logger configuration:

```
INFO [Main] 'AppBootstrapper'  ______               __          __                                     
INFO [Main] 'AppBootstrapper' |   __ \.-----.-----.|  |_.-----.|  |_.----.---.-.-----.-----.-----.----.
INFO [Main] 'AppBootstrapper' |   __ <|  _  |  _  ||   _|__ --||   _|   _|  _  |  _  |  _  |  -__|   _|
INFO [Main] 'AppBootstrapper' |______/|_____|_____||____|_____||____|__| |___._|   __|   __|_____|__|  
INFO [Main] 'AppBootstrapper'                                                  |__|  |__|              
INFO [Main] 'AppBootstrapper' 
INFO [Main] 'AppBootstrapper' Assembly location: C:\Projects\Bootstrapper\src\Bootstrapper\bin\Debug
INFO [Main] 'AppBootstrapper'  Assembly version: 1.0.0.0
INFO [Main] 'AppBootstrapper'      File version: 1.0.0.0
INFO [Main] 'AppBootstrapper'   Product version: 1.0.0.0
INFO [Main] 'AppBootstrapper'        Running as: DESKTOP-DUF3FP2\kmcgi
```

## Metrics recorder

Not every app uses metrics, but it should. How do you truly know if a feature is useful to your users without measuring it. The alternative is to stand over their shoulder 8 hours a day, 5 days a week. Or just keep sweating features that no one uses, wasting your employer thousands of dollars.

In this category, simple is better. I really like StatsD. It pushes metrics to the server over UDP which is a very low overhead protocol. Plus, if the server is down, the app just keeps on trucking. Why should we care if we lose a few metrics here and there. They're nice, but not worth shutting down the business when we can't get them.

Just like in logging, set up your own abstraction over whatever library you choose. The interface should be low ceremony; just a string

## Handling unhandled exceptions

Next, you should set up handlers for unhandled exceptions. Every framework/library has different ways of handling this, so figure out what they are and log them at the very least (this is what the FATAL log level is for). Doing this as early as possible allows you to find issues during your bootstrapping process. They happen. Trust me.

Also, don't let your app continue to run after one of these exceptions. If the exception you are getting is truly not fatal then it should be getting caught somewhere up the stack. These handlers are strictly for the unexpected, and after one happens there's no telling what state your application is in. It's better to just tear it all down and try again.

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

## Register IoC components

Next, we start stitching our app together with our inversion of control container. Pick your poison; they're all good. Find one that allows you to use the design patterns you want to use. For example, very few containers have first class support for generic decorators, which is an extremely powerful pattern when used correctly. Do not fall into the trap of picking the container with a lot of features outside of simply dependency resolution and controlling lifetimes. That's their only job.

Also, take advantage of the convention based registration. If you are building an Asp.net Web API app, then all of your controllers must be transient. Take advantage of these little patterns to make your life easier.

## Run database migrations

I'm a strong believer in database migrations that are run inside your application. This gives you immense control over the process. If you are using Entity Framework migrations, do yourself a favor and write your own `IDatabaseInitializer<>`. This allows you to log every step of the process, backup existing data, and perform database changes that EF migrations doesn't allow out of the box.

```c#
public class BootstrappingDatabaseInitializer : IDatabaseInitializer<BootstrappingDataContext>
{
    public void InitializeDatabase(BootstrappingDataContext context)
    {
        var dbExists = context.Database.Exists();
        var dbLocation = context.Database.Connection.DataSource;
        this.Log().Info(string.Format("Database location: {0}", dbLocation));

        if (!dbExists)
        {
            this.Log().Info(string.Format("Creating database at {0}", dbLocation));
            CopyResource("Bootstrapping.sdf", dbLocation);
        }

        var configuration = new Configuration();
        var migrator = new DbMigrator(configuration);

        var pendingMigrations = migrator.GetPendingMigrations().ToArray();

        if (dbExists && pendingMigrations.Any())
        {
            BackupDatabase(dbLocation);
        }

        try
        {
            this.Log().Info(string.Format("Running migrations on {0}...", context.Database.Connection.DataSource));
            foreach (var pendingMigration in pendingMigrations)
            {
                this.Log().Info(string.Format("Preparing database migration: {0}", pendingMigration));
                migrator.Update(pendingMigration);
            }
            if (!dbExists && pendingMigrations.NotAny())
            {
                Configuration.SeedDatabase(context);
            }
            this.Log().Info("Finished running migrations...");
        }
        catch (SqlCeException e)
        {
            this.Log().Error(e, "Unable to execute database migrations.");
        }
    }

    static void BackupDatabase(string existingDb)
    {
        var basePath = Path.GetDirectoryName(existingDb);
        var baseFileName = Path.GetFileName(existingDb);
        var backupPath = Path.Combine(basePath, "Backups");
        Directory.CreateDirectory(backupPath);

        var backupFileName = string.Concat(
            Path.GetFileNameWithoutExtension(baseFileName),
            "_",
            DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            Path.GetExtension(baseFileName)
            );
        
        var backupFullPath = Path.Combine(backupPath, backupFileName);
        File.Copy(existingDb, backupFullPath, true);
    }

    void CopyResource(string resourceName, string file)
    {
        using (var resource = GetType().Assembly.GetManifestResourceStream(resourceName))
        {
            if (resource == null)
            {
                throw new ArgumentException("resourceName", "No such resource");
            }
            using (Stream output = File.OpenWrite(file))
            {
                resource.CopyTo(output);
            }
        }
    }
}
```
