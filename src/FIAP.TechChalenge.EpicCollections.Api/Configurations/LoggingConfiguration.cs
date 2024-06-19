using Serilog;

namespace FIAP.TechChalenge.EpicCollections.Api.Configurations
{
    public static class LoggingConfiguration
    {
        public static void AddLoggingConfiguration(this IHostBuilder host)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/epicollections.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            host.UseSerilog();
        }
    }
}
