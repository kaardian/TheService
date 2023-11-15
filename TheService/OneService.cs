using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using TheService.Properties;
using TheService.Models;

namespace TheService
{
    public partial class OneService : ServiceBase
    {
        // Instantiate the service environment.
        public static ServiceEnvironment ServiceEnvironment { get; set; }

        /// <summary>
        /// Constructor, start of the applicaiton.
        /// </summary>
        public OneService()
        {
            // Uncomment this for debugging the launch of the service.
            // System.Diagnostics.Debugger.Launch();

            InitializeComponent();
            this.ServiceName = Settings.Default.ServiceName;
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = false;

            // Initialize the Event Logs.
            ((ISupportInitialize)this.EventLog).BeginInit();
            if (!EventLog.SourceExists(this.ServiceName))
                EventLog.CreateEventSource(this.ServiceName, Logger.LogLocation);

            ((ISupportInitialize)this.EventLog).EndInit();

            // Set the Event Log service and log name.
            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = Logger.LogLocation;

            // Initialize the service environment.
            try
            {
                ServiceEnvironment = new ServiceEnvironment();
                ServiceEnvironment.EventLog = this.EventLog;
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry($"Failed to initialize service environment.\n" +
                    $"{ex.Message}\n" +
                    $"{ex.StackTrace}",
                    type: EventLogEntryType.Error);
                Environment.Exit(0);
            }

            // Apply localization.
            try
            {
                ServiceEnvironment.LocalizedMessages = Localization.ReadLocalization();
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry($"Failed to read localization. Using default values.\n" +
                    $"{ex.Message}\n" +
                    $"{ex.StackTrace}",
                    type: EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Start service.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            ServiceEnvironment.EventLog.WriteEntry(message: ServiceEnvironment.LocalizedMessages.ServiceStart, type: EventLogEntryType.Information);
            
            // If debugging is enabled, and debugger is not connected, launch the debugger.
            if (Settings.Default.LaunchDebugger &&
                !Debugger.IsAttached)
            {
                ServiceEnvironment.EventLog.WriteEntry(ServiceEnvironment.LocalizedMessages.LaunchDebugger);
                System.Diagnostics.Debugger.Launch();
            }

            // Attempt starting the Heartbeat. If this fails, will also shutdown the service.
            try
            {
                HeartBeat.StartBeat(ServiceEnvironment);
            }
            catch (Exception ex) {
                ServiceEnvironment.EventLog.WriteEntry(message: ServiceEnvironment.LocalizedMessages.HeartBeat.StartFailed +
                    $"{ex.Message}\n" +
                    $"{ex.StackTrace}",
                    type: EventLogEntryType.Error);
                Environment.Exit(0);
            }

            ServiceEnvironment.EventLog.WriteEntry(message: ServiceEnvironment.LocalizedMessages.ServiceStarted, type: EventLogEntryType.Information);
        }

        /// <summary>
        /// Stop service.
        /// </summary>
        protected override void OnStop()
        {
            HeartBeat.StopBeat(ServiceEnvironment);
            ServiceEnvironment.EventLog.WriteEntry(message: ServiceEnvironment.LocalizedMessages.ServiceStop, type: EventLogEntryType.Information);
        }
    }
}
