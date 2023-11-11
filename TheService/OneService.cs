using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TheService.Properties;
using TheService.Models;

namespace TheService
{
    public partial class OneService : ServiceBase
    {
        /// <summary>
        /// Constructor, start of the applicaiton.
        /// </summary>
        public OneService()
        {
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
        }

        /// <summary>
        /// Start service.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            this.EventLog.WriteEntry("Starting The One Service");
            
            // If debugging is enabled, and debugger is not connected, launch the debugger.
            if (Settings.Default.LaunchDebugger &&
                !Debugger.IsAttached)
            {
                this.EventLog.WriteEntry("Launching debugger");
                System.Diagnostics.Debugger.Launch();
            }
        }

        /// <summary>
        /// Stop service.
        /// </summary>
        protected override void OnStop()
        {
            this.EventLog.WriteEntry("Stopping The One Service");
        }
    }
}
