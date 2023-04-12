namespace WellsFargo.OMS.Library.Loggers
{
    public interface ILogger
    {
        void LogError(string message);
        void LogInformation(string message);
        void LogDebug(string message);
    }
}
