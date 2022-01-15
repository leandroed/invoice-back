using Serilog;

namespace InvoiceApi;

/// <summary>
/// Logger configuration class.
/// </summary>
public static class LoggerConfig
{
    /// <summary>
    /// Initialize application logger.
    /// </summary>
    public static void InitializeLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs\\logApplication.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
