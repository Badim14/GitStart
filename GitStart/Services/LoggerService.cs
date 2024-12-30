using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitStart.Services
{
    public class LoggerService
    {
        public static ILogger Logger { get; set; }

        static LoggerService()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.File("Logs-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        public static void LogInfo(string message, params object[] args)
        {
            Logger.Information(message, args);
        }

        public static void LogError(Exception ex, string message, params object[] args)
        {
            Logger.Error(ex, message, args);
        }

        public static void LogWarning(string message, params object[] args)
        {
            Logger.Warning(message, args);
        }
    }
}

