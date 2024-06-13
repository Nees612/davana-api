using System.Runtime.CompilerServices;
using Serilog.Context;

namespace API.Logging
{
    public static class LoggerHelper
    {
        public static void LogAppError(this ILogger logger, Exception exception, string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string sourceFilePath = "", params object[] args)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(sourceFilePath);
            using var prop = LogContext.PushProperty("CodeTrace", $"[{callerClassName}][{callerMemberName}]");
            logger.LogError(exception, message, args);
        }

        public static void LogAppWarning(this ILogger logger, string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string sourceFilePath = "", params object[] args)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(sourceFilePath);
            using var prop = LogContext.PushProperty("CodeTrace", $"[{callerClassName}][{callerMemberName}]");
            logger.LogError(message, args);
        }
        public static void LogAppInfo(this ILogger logger, string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string sourceFilePath = "", params object[] args)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(sourceFilePath);
            using var prop = LogContext.PushProperty("CodeTrace", $"[{callerClassName}][{callerMemberName}]");
            logger.LogError(message, args);
        }
        public static void LogAppDebug(this ILogger logger, string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string sourceFilePath = "", params object[] args)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(sourceFilePath);
            using var prop = LogContext.PushProperty("CodeTrace", $"[{callerClassName}][{callerMemberName}]");
            logger.LogError(message, args);
        }
    }
}