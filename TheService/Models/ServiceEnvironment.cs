using System.Diagnostics;

namespace TheService.Models
{
    public class ServiceEnvironment
    {
        public Messages LocalizedMessages { get; set; }
        public EventLog EventLog { get; set; }

        /// <summary>
        /// Service environment constructor.
        /// </summary>
        public ServiceEnvironment() {

            // Instantiate the localized messages.
            LocalizedMessages = new Messages();
        }
    }
}
