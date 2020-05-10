using System.Collections.Generic;

namespace DeviceManagementConsole.Shared
{
    public class StatusReport : KeepaliveReport
    {
        public string ClientVersion { get; set; }

        // -1: Unknown error
        // 0: running
        // 1: closeing by user
        // 2: shutting down
        // 3: Application stopped by self
        public int Status { get; set; }

        public OSInfo OS { get; set; }

        public PerformanceInfo Performance { get; set; }

        public IEnumerable<ProcessInfo> Processes { get; set; }

        public IEnumerable<ServiceInfo> Services { get; set; }

        public class PerformanceInfo
        {
            public float CpuPerc { get; set; }

            public float RamMb { get; set; }

            public ulong Uptime { get; set; }
        }

        public class ProcessInfo
        {
            public ProcessInfo(int pid, string name)
            {
                this.Pid = pid;
                this.Name = name;
            }

            public int Pid { get; set; }

            public string Name { get; set; }

            public string User { get; set; }

            public string CommandLine { get; set; }
        }

        public class ServiceInfo
        {
            public ServiceInfo(string name)
            {
                this.Name = name;
            }

            public string Name { get; set; }

            public string Status { get; set; }
        }

        public class OSInfo
        {
            public string Name { get; set; }

            public string Version { get; set; }
        }
    }
}