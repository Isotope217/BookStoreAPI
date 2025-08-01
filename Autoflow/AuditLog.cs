namespace Autoflow;

public static class AuditLog
{
    private static readonly object _lock = new();
    private static readonly List<string> _logMessages = new();

    public static void AddToLog(string message)
    {
        lock (_lock)
        {
            _logMessages.Add($"{DateTime.UtcNow:O} - {message}");
        }
    }

    public static IReadOnlyList<string> GetLogMessages()
    {
        lock (_lock)
        {
            return _logMessages.ToList();
        }
    }

    public static void Clear()
    {
        lock (_lock)
        {
            _logMessages.Clear();
        }
    }
}