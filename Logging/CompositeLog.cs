public abstract class CompositeLog<TLogger> : ILog, ILog<TLogger>
{
    readonly List<ILog> _loggers;

    protected CompositeLog(params ILog[] loggers)
    {
        _loggers = loggers.ToList();
    }

    public void InitializeFor(string loggerName)
    {
        _loggers.ForEach(x => x.InitializeFor(loggerName));
    }

    public void Trace(string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Trace(message, formatting));
    }

    public void Trace(Func<string> message)
    {
        _loggers.ForEach(x => x.Trace(message));
    }

    public void Debug(string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Debug(message, formatting));
    }

    public void Debug(Func<string> message)
    {
        _loggers.ForEach(x => x.Debug(message));
    }

    public void Info(string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Info(message, formatting));
    }

    public void Info(Func<string> message)
    {
        _loggers.ForEach(x => x.Info(message));
    }

    public void Warn(string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Warn(message, formatting));
    }

    public void Warn(Func<string> message)
    {
        _loggers.ForEach(x => x.Warn(message));
    }

    public void Warn(Exception exception, string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Warn(exception, message, formatting));
    }

    public void Warn(Func<string> message, Exception exception)
    {
        _loggers.ForEach(x => x.Warn(message, exception));
    }

    public void Error(string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Error(message, formatting));
    }

    public void Error(Func<string> message)
    {
        _loggers.ForEach(x => x.Error(message));
    }

    public void Error(Exception exception, string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Error(exception, message, formatting));
    }

    public void Error(Func<string> message, Exception exception)
    {
        _loggers.ForEach(x => x.Error(message, exception));
    }

    public void Fatal(string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Fatal(message, formatting));
    }

    public void Fatal(Func<string> message)
    {
        _loggers.ForEach(x => x.Fatal(message));
    }

    public void Fatal(Exception exception, string message, params object[] formatting)
    {
        _loggers.ForEach(x => x.Fatal(exception, message, formatting));
    }

    public void Fatal(Func<string> message, Exception exception)
    {
        _loggers.ForEach(x => x.Fatal(message, exception));
    }
}
