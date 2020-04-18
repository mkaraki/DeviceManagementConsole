using System.Collections.Generic;

namespace DeviceManagementConsole.Shared
{
    public class StatusReport : KeepaliveReport
    {
        public OSInfo os { get; set; }

        public PerformanceInfo performance { get; set; }

        public IEnumerable<ProcessInfo> processes { get; set; }

        public IEnumerable<ServiceInfo> services { get; set; }

        public class PerformanceInfo 
        {
            public float cpuPerc { get; set; }

            public float ramMb { get; set; }
        }

        public class ProcessInfo
        {
            public ProcessInfo(int pid, string name)
            {
                this.pid = pid;
                this.name = name;
            }

            public int pid { get; set; }

            public string name { get; set; }

            public string user { get; set; }

            public string commandLine { get; set; }
        }

        public class ServiceInfo
        {
            public ServiceInfo(string name)
            {
                this.name = name;
            }

            public string name { get; set; }

            public string status { get; set; }
        }

        public class OSInfo
        {
            public string name { get; set; }

            public string version { get; set; }
        }
    }
}