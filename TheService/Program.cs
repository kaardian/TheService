using System.ServiceProcess;

namespace TheService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new OneService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
