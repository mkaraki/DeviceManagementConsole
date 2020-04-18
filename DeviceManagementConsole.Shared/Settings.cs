using System;

namespace DeviceManagementConsole.Shared
{
    public class Settings
    {
        public string ComputerUnique { get; set; } = null;

        public string Server { get; set; } = "dmc.local";

        public int Port { get; set; } = 2515;

        public int Interval { get; set; } = 60000;

        public bool KeepaliveEnabled { get; set; } = false;

        public int KeepaliveInterval { get; set; } = 30000;

        public bool TaskEnabled { get; set; } = true;

        public int TaskInterval { get; set; } = 30000;
    }
}
