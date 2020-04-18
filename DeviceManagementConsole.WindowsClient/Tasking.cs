using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceManagementConsole.WindowsClient
{
    public static class Tasking
    {
        [DllImport("user32.dll")]
        private static extern bool LockWorkStation();

        public enum ExitWindows : uint
        {
            EWX_LOGOFF = 0x00,
            EWX_SHUTDOWN = 0x01,
            EWX_REBOOT = 0x02,
            EWX_POWEROFF = 0x08,
            EWX_RESTARTAPPS = 0x40,
            EWX_FORCE = 0x04,
            EWX_FORCEIFHUNG = 0x10,
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ExitWindowsEx(ExitWindows uFlags,
            int dwReason);

        public static bool RunTask(Shared.RemoteTask task)
        {
            switch (task.type)
            {
                case "tell":
                    Tell(task.options.FirstOrDefault() ?? string.Empty);
                    return true;

                case "lock":
                    Lock();
                    return true;

                case "shutdown":
                    Shutdown();
                    return true;

                case "reboot":
                    Reboot();
                    return true;

                case "stop":
                    Stop();
                    return true;
            }

            return false;
        }

        private static void Tell(string message)
        {
            Task.Run(() => MessageBox.Show(message, "Message from Manager", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification));
        }

        private static void Lock()
        {
            LockWorkStation();
        }

        private static void Shutdown()
        {
            ExitWindowsEx(ExitWindows.EWX_SHUTDOWN, 0);
        }

        private static void Reboot()
        {
            ExitWindowsEx(ExitWindows.EWX_REBOOT, 0);
        }

        private static void Stop()
        {
            Main.DenyStop = false;
            Environment.Exit(0);
        }
    }
}
