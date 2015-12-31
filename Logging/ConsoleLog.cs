/// <summary>
/// A logger that writes to Console.Out.
/// </summary>
public class ConsoleLog : DefaultBaseLog<ConsoleLog>
{
    protected override void Write(string level, string message, Exception exception = null)
    {
        var formattedMessage = FormatMessage(level, message, exception);
        Console.WriteLine(formattedMessage);
    }
}
