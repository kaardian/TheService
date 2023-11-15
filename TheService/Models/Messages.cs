using System.Runtime.Serialization;

namespace TheService.Models
{
    [DataContract]
    public class Messages
    {
        [DataMember]
        public string ServiceStart { get; set; }
        
        [DataMember]
        public string ServiceStop { get; set; }
        
        [DataMember]
        public string ServiceStarted { get; set; }
        
        [DataMember]
        public string LaunchDebugger { get; set; }

        [DataMember]
        public HeartBeatMessages HeartBeat { get; set; }

        public Messages()
        {
            // Set the default English values in the constructor.
            ServiceStart = "Starting The One Service";
            ServiceStop = "Stopping The One Service";
            ServiceStarted = "Started The One Service";
            LaunchDebugger = "Launching debugger";
            HeartBeat = new HeartBeatMessages
            {
                Start = "Starting heartbeats",
                Stop = "Stopping heartbeats",
                Tick = "Heartbeat Tick",
                StartFailed = "Heartbeat start failed"
            };
        }

        [DataContract]
        public class HeartBeatMessages
        {
            [DataMember]
            public string Start { get; set; }

            [DataMember]
            public string Stop { get; set; }

            [DataMember]
            public string Tick { get; set; }

            [DataMember]
            public string StartFailed { get; set; }
        }
    }
}