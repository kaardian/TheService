using System.Runtime.Serialization;

namespace TheService.Models
{
    [DataContract]
    public class Messages
    {
        [DataMember]
        public string ServiceStart { get; set; } = "Starting The One Service";

        [DataMember]
        public string ServiceStop { get; set; } = "Stopping The One Service";

        [DataMember]
        public string ServiceStarted { get; set; } = "Started The One Service";

        [DataMember]
        public string LaunchDebugger { get; set; } = "Launching debugger";

        [DataMember]
        public HeartBeatMessages HeartBeat { get; set; }

        [DataMember]
        public AutoUpdateMessages AutoUpdate { get; set; }

        public Messages()
        {
            HeartBeat = new HeartBeatMessages();
            AutoUpdate = new AutoUpdateMessages();
        }
    }
}