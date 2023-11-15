using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using TheService.Properties;
using System.Runtime.Serialization;

namespace TheService.Models
{
    [DataContract]
    public class AutoUpdateMessages
    {
        [DataMember]
        public string Start { get; set; } = "Starting auto update service";

        [DataMember]
        public string Stop { get; set; } = "Stopping auto update service";

        [DataMember]
        public string UpdateCycle { get; set; } = "Auto update cycle starting";

        [DataMember]
        public string StartFailed { get; set; } = "Auto update start failed";

        [DataMember]
        public string UpdateCycleRunning { get; set; } = "Auto update cycle running already, skipping this cycle to let the previous one finish";

        [DataMember]
        public string UpdatesNotEnabled { get; set; } = "Auto updates are not enabled, skipping update cycle";

        [DataMember]
        public string UpdateCycleComplete { get; set; } = "Auto updates cycle completed";
    }

    public static class AutoUpdate
    {
        /// <summary>
        /// Defines how often to check configuration updates, defined in seconds.
        /// </summary>
        public static int UpdateInterval { get; set; }
        public static DateTime LastUpdated { get; set; }
        public static DateTime NextUpdate { get; set; }

        /// <summary>
        /// Defines if the automatic updates are enabled or not.
        /// </summary>
        public static bool Enabled { get; set; }
        
        /// <summary>
        /// Defines if the update is currently running.
        /// </summary>
        public static bool Updating { get; set; }

        internal static CancellationTokenSource wtoken;
        internal static Task task;

        /// <summary>
        /// Auto update service stop.
        /// </summary>
        /// <param name="serviceEnvironment"></param>
        internal static void Stop(ServiceEnvironment serviceEnvironment)
        {
            serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.AutoUpdate.Stop, EventLogEntryType.Information);

            wtoken.Cancel();

            try
            {
                task.Wait();
            }
            catch (AggregateException) { }
        }

        /// <summary>
        /// Auto update service start.
        /// </summary>
        /// <param name="serviceEnvironment"></param>
        internal static void Start(ServiceEnvironment serviceEnvironment)
        {
            serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.AutoUpdate.Start, EventLogEntryType.Information);

            // Set the last updated timw to now.
            LastUpdated = DateTime.Now;
            UpdateInterval = Settings.Default.UpdateInterval;
            NextUpdate = DateTime.Now.AddSeconds(UpdateInterval);
            Enabled = Settings.Default.AutoUpdateEnabled;
            Updating = false;

            wtoken = new CancellationTokenSource();

            int interval = UpdateInterval;

            task = Task.Run(async () =>  // <- marked async
            {
                while (true)
                {
                    UpdateCycle(serviceEnvironment);
                    await Task.Delay(interval.SecondsToMilliseconds(), wtoken.Token); // <- await with cancellation
                }
            }, wtoken.Token);
        }

        /// <summary>
        /// Auto update check updates cycle.
        /// </summary>
        /// <param name="serviceEnvironment"></param>
        private static void UpdateCycle(ServiceEnvironment serviceEnvironment)
        {
            serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.AutoUpdate.UpdateCycle, EventLogEntryType.Information);

            // Check if the update is already running, and has has not completed. If so, skip this cycle.
            if (Updating)
            {
                serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.AutoUpdate.UpdateCycleRunning, EventLogEntryType.Information);
                return;
            }

            // Check if the updates are enabled. If not, skip this cycle.
            if (!Enabled)
            {
                serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.AutoUpdate.UpdatesNotEnabled, EventLogEntryType.Information);
                return;
            }

            // Not updating, and enabled. Proceed to run the auto update.
            LastUpdated = DateTime.Now;
            Updating = true;

            // TODO: Fetch new configuration set, and check if there is an update for any used moduled.
            Thread.Sleep(70.SecondsToMilliseconds());

            Updating = false;
            NextUpdate = DateTime.Now.AddSeconds(UpdateInterval);

            serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.AutoUpdate.UpdateCycleComplete, EventLogEntryType.Information);
        }
    }
}
