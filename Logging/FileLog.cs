/// <summary>
/// A logger that appends the log message to a given file.
/// </summary>
public class FileLog : DefaultBaseLog<ConsoleLog>
{
    readonly string _logFilePath;

    public FileLog()
    {
        _logFilePath = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? String.Empty, "WpfVlcPlayer.log");
    }

    protected void EnsurePathExists(string filePath)
    {
        var directory = Path.GetDirectoryName(_logFilePath) ?? String.Empty;

        if (!String.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }

    protected virtual void WriteLineToFile(string formattedMessage)
    {
        EnsurePathExists(_logFilePath);
        
        var encodedBytes = Encoding.UTF8.GetBytes(formattedMessage + Environment.NewLine);

        using (var fileStream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        {
            fileStream.Write(encodedBytes, 0, encodedBytes.Length);
            fileStream.Flush();
        }
    }

    protected override void Write(string level, string message, Exception exception = null)
    {
        var formattedMessage = FormatMessage(level, message, exception);
        WriteLineToFile(formattedMessage);
    }
}
