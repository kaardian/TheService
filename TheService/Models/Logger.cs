using TheService.Properties;

namespace TheService.Models
{
    /// <summary>
    /// Logger class that sets up logging methods and values etc.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Defines the service name to log. Uses the Service name from settings.
        /// </summary>
        public static string LogSource { get { return Settings.Default.ServiceName; } }

        /// <summary>
        /// Defines the log location. Uses Windows Event Logs Application Log.
        /// </summary>
        public static string LogLocation { get { return "Application"; } }
    }
}
