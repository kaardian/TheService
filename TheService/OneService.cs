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
        // Instantiate the localized messages.
        public static Messages LocalizedMessages;

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

            // Initialize the localization.
            try
            {
                LocalizedMessages = new Messages();
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry($"Failed to initialize localization.\n" +
                    $"{ex.Message}\n" +
                    $"{ex.StackTrace}",
                    type: EventLogEntryType.Error);
                Environment.Exit(0);
            }

            // Apply localization.
            try
            {
                LocalizedMessages = Localization.ReadLocalization();
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry($"Failed to read localization.\n" +
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

            this.EventLog.WriteEntry(message: LocalizedMessages.ServiceStart, type: EventLogEntryType.Information);
            
            // If debugging is enabled, and debugger is not connected, launch the debugger.
            if (Settings.Default.LaunchDebugger &&
                !Debugger.IsAttached)
            {
                this.EventLog.WriteEntry(LocalizedMessages.LaunchDebugger);
                System.Diagnostics.Debugger.Launch();
            }

            this.EventLog.WriteEntry(message: LocalizedMessages.ServiceStarted, type: EventLogEntryType.Information);
        }

        /// <summary>
        /// Stop service.
        /// </summary>
        protected override void OnStop()
        {
            this.EventLog.WriteEntry(message: LocalizedMessages.ServiceStop, type: EventLogEntryType.Information);
        }
    }
}
