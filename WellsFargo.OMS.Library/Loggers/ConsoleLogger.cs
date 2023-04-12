using System;

namespace WellsFargo.OMS.Library.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void LogDebug(string message)
        {
            Console.WriteLine($"[DEBUG] {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        public void LogInformation(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }
    }
}
