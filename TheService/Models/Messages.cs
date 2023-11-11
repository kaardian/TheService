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

        public Messages()
        {
            // Set the default English values in the constructor.
            ServiceStart = "Starting The One Service";
            ServiceStop = "Stopping The One Service";
            ServiceStarted = "Started The One Service";
            LaunchDebugger = "Launching debugger";
        }
    }
}