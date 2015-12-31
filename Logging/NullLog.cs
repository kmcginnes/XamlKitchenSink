/// <summary>
/// The default logger until one is set.
/// </summary>
public class NullLog : DefaultBaseLog<NullLog>
{
    protected override void Write(string level, string message, Exception exception = null)
    {
    }
}
