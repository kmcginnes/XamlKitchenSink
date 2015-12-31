public abstract class DefaultBaseLog<TLog> : ILog, ILog<TLog>
{
    private string _loggerName;

    protected string FormatMessage(string level, string message, Exception exception = null)
    {
        var timestamp = DateTime.Now.ToLongTimeString();
        var threadName = Thread.CurrentThread.Name;
        if (String.IsNullOrEmpty(threadName))
            threadName = Thread.CurrentThread.ManagedThreadId.ToString();

        var shortLoggerName = _loggerName.Split('.').LastOrDefault() ?? "None";

        var prefix = String.Format("{0} {1,5} [{2}][{3}]", timestamp, level, threadName, shortLoggerName);

        var result = String.Format("{0}  {1}", prefix, message);

        if (exception != null)
        {
            result = result + String.Format(
                "{0}{1}: {2} {3}",
                Environment.NewLine, exception.GetType().FullName, exception.Message, exception.StackTrace);
        }
        return result;
    }

    protected abstract void Write(string level, string message, Exception exception = null);

    public void InitializeFor(string loggerName)
    {
        _loggerName = loggerName;
    }

    public void Trace(string message, params object[] formatting)
    {
        Write("TRACE", String.Format(message, formatting));
    }

    public void Trace(Func<string> message)
    {
        Write("TRACE", message());
    }

    public void Debug(string message, params object[] formatting)
    {
        Write("DEBUG", String.Format(message, formatting));
    }

    public void Debug(Func<string> message)
    {
        Write("DEBUG", message());
    }

    public void Info(string message, params object[] formatting)
    {
        Write("INFO", String.Format(message, formatting));
    }

    public void Info(Func<string> message)
    {
        Write("INFO", message());
    }

    public void Warn(string message, params object[] formatting)
    {
        Write("WARN", String.Format(message, formatting));
    }

    public void Warn(Func<string> message)
    {
        Write("WARN", message());
    }

    public void Warn(Exception exception, string message, params object[] formatting)
    {
        Write("WARN", String.Format(message, formatting), exception);
    }

    public void Warn(Func<string> message, Exception exception)
    {
        Write("WARN", message(), exception);
    }

    public void Error(string message, params object[] formatting)
    {
        Write("ERROR", String.Format(message, formatting));
    }

    public void Error(Func<string> message)
    {
        Write("ERROR", message());
    }

    public void Error(Exception exception, string message, params object[] formatting)
    {
        Write("ERROR", String.Format(message, formatting), exception);
    }

    public void Error(Func<string> message, Exception exception)
    {
        Write("ERROR", message(), exception);
    }

    public void Fatal(string message, params object[] formatting)
    {
        Write("FATAL", String.Format(message, formatting));
    }

    public void Fatal(Func<string> message)
    {
        Write("FATAL", message());
    }

    public void Fatal(Exception exception, string message, params object[] formatting)
    {
        Write("FATAL", String.Format(message, formatting), exception);
    }

    public void Fatal(Func<string> message, Exception exception)
    {
        Write("FATAL", message(), exception);
    }
}
