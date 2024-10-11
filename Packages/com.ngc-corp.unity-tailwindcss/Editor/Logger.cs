using System.Diagnostics;
using System.Reflection;

namespace NGCCorp.TailwindCSS
{
  public static class Logger
  {
    // Log Info level message
    public static void LogInfo(string message)
    {
      string logMessage = FormatMessage(message);
      UnityEngine.Debug.Log(logMessage);
    }

    // Log Warning level message
    public static void LogWarning(string message)
    {
      string logMessage = FormatMessage(message);
      UnityEngine.Debug.LogWarning(logMessage);
    }

    // Log Error level message
    public static void LogError(string message)
    {
      string logMessage = FormatMessage(message);
      UnityEngine.Debug.LogError(logMessage);
    }

    // Helper method to format the log message
    private static string FormatMessage(string message)
    {
      // Get the stack trace and retrieve the frame that contains the caller
      StackTrace stackTrace = new StackTrace();

      // Start at frame 2 (since frame 0 and 1 are inside the logger itself)
      MethodBase callingMethod = stackTrace.GetFrame(2).GetMethod();

      // Get the class and method name from the calling method
      string className = callingMethod.DeclaringType?.Name ?? "UnknownClass";
      string methodName = callingMethod.Name;

      return $"[{className}.{methodName}: {message}";
    }
  }
}
