namespace Autoflow;

public static class AuditLog
{
    public static List<string> LogMessages;
    
    public static void AddToLog(string message)
    {
        LogMessages ??= new List<string>();

        LogMessages.Add(message);
    }
}