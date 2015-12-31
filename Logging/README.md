# Logging

I've seen logging done in a lot of different ways over the years. But there's one setup that I've favored over all the others.

Typically you see something like this:

```c#
class Foo
{
  readonly ILogger _logger = LogManager.GetLoggerFor(typeof(Foo));
  
  public void Bar()
  {
    _logger.Info("Bar did something awesome!");
  }
}
```

Every class that needs logging will request a logger from the `LogManager`. The problem I have with this is that it tightly couples every single class to your logging system, whether it be log4net, NLog, or something else.

An alternative I've seen is getting the logger through constructor injection:

```c#
class Foo
{
  readonlyILogger _logger;
  
  public Foo(ILogger logger)
  {
    _logger = logger;
  }
  
  public void Bar()
  {
    _logger.Info("Bar did something awesome!");
  }
}
```

This removes the `LogManager.GetLoggerFor()` call and pushes it into the IoC container. This is a decent improvement, but really, the logger is an optional dependency. We generally use constructor injection to make testing easier by mocking out dependencies. I generally don't care about mocking out a logger. So this is just noise.

## this.Log

My favorite approach is to use an extension method off of `object` that retrieves the logger:

```c#
class Foo
{
  public void Bar()
  {
    this.Log().Info("Bar did something awesome!");
  }
}
```

Not only is this way shorter than both the options above, it is also **always** available. If I'm in a class that has never logged before I don't have to worry about setting up a field and getting the logger instance, I just keep typing. I never have to leave the context of the code I'm trying to write. And that is a very good thing.

### Configuration

So how does this work? Easy.

There's an abstraction of your typical log methods in `ILog`. This is all your application will need to know about. Therefore all dependencies on log4net or NLog are pushed into application bootstrapper. In fact, if you don't do anything in the bootstrapper you'll just get a `NullLog` instance that does nothing.

Alternatively, you can set up `this.Log()` to return any logger you want:

```c#
Log.InitializeWith<ConsoleLog>();
// or
Log.InitializeWith<Log4netLog>();
// or
```

### Static methods

What if you're in a static method or class? You would not be able to use `this.Log()` because there is no `this`. Correct. But where there's a will, there's a way.

```c#
static class Foo
{
  public static void Bar()
  {
    // Does not work in static methods
    // this.Log().Info("Bar did something awesome!");
    
    // However, this will work just fine
    typeof(Foo).Log().Info("Bar did something awesome!");
    
    // Or this
    "Foo".Log().Info("Bar did something awesome!");
  }
}
```

And for extension methods, always dot off the first parameter that represents the object being extended.

```c#
static class FooExtensions
{
  public static void Bar(this Foo foo)
  {
    foo.Log().Info("Bar did something awesome!");
  }
}
```

## Source of inspiration

This all started after I read ferventcoder's [post on CodeProject](http://www.codeproject.com/Articles/530808/Introducingplusthis-Log) about this pattern. Ever since I've been using a slightly customized version of his original idea.

I also heard that ReactiveUI and Splat have an implementation of this idea as well. I even think that it came first. But I prefer my own implementation for a couple of reasons.

First, it doesn't depend on anything to just work. Second, I can bring in the source code, which means the extension method lives within my app's namespace. I don't have to add a `using` statement just for logging. It seems minor, but I consider that a big win.
