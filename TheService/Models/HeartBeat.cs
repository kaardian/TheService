using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TheService.Properties;

namespace TheService.Models
{
    [DataContract]
    public class HeartBeatMessages
    {
        [DataMember]
        public string Start { get; set; } = "Starting heartbeats";

        [DataMember]
        public string Stop { get; set; } = "Stopping heartbeats";

        [DataMember]
        public string Tick { get; set; } = "Heartbeat Tick";

        [DataMember]
        public string StartFailed { get; set; } = "Heartbeat start failed";
    }

    internal static class HeartBeat
    {
        internal static CancellationTokenSource wtoken;
        internal static Task task;

        /// <summary>
        /// Heartbeat stop.
        /// </summary>
        /// <param name="serviceEnvironment"></param>
        internal static void StopBeat(ServiceEnvironment serviceEnvironment)
        {
            serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.HeartBeat.Stop, EventLogEntryType.Information);

            wtoken.Cancel();

            try
            {
                task.Wait();
            }
            catch (AggregateException) { }
        }

        /// <summary>
        /// Heartbeat taks start.
        /// </summary>
        /// <param name="serviceEnvironment"></param>
        internal static void StartBeat(ServiceEnvironment serviceEnvironment)
        {
            serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.HeartBeat.Start, EventLogEntryType.Information);
            
            wtoken = new CancellationTokenSource();

            int interval = Settings.Default.HeartBeat;

            task = Task.Run(async () =>  // <- marked async
            {
                while (true)
                {
                    Beat(serviceEnvironment);
                    await Task.Delay(interval.SecondsToMilliseconds(), wtoken.Token); // <- await with cancellation
                }
            }, wtoken.Token);
        }

        /// <summary>
        /// Heartbeat tick.
        /// </summary>
        /// <param name="serviceEnvironment"></param>
        private static void Beat(ServiceEnvironment serviceEnvironment)
        {
            serviceEnvironment.EventLog.WriteEntry(serviceEnvironment.LocalizedMessages.HeartBeat.Tick, EventLogEntryType.Information);
        }
    }
}
