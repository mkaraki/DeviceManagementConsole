using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YamlDotNet.Serialization;

namespace DeviceManagementConsole.WindowsClient
{
    internal static class Program
    {
        internal static Shared.Settings Config;

        internal static string Version { get; private set; } = "Develop 0";

        internal static Shared.StatusReport.OSInfo OSInfo { get; private set; }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out long lpLuid);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public long Luid;
            public int Attributes;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        public static void AdjustToken()
        {
            const uint TOKEN_ADJUST_PRIVILEGES = 0x20;
            const uint TOKEN_QUERY = 0x8;
            const int SE_PRIVILEGE_ENABLED = 0x2;
            const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return;

            IntPtr procHandle = GetCurrentProcess();

            OpenProcessToken(procHandle, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out IntPtr tokenHandle);
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            tp.Attributes = SE_PRIVILEGE_ENABLED;
            tp.PrivilegeCount = 1;
            LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out tp.Luid);
            AdjustTokenPrivileges(tokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);

            CloseHandle(tokenHandle);
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string reason);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int SetProcessShutdownParameters(int dwLevel, int dwFlags);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if DEBUG
            Version += " [DEBUG BUILD]";
#endif

            if (File.Exists(@"C:\dmc.yaml"))
            {
                string conf = File.ReadAllText(@"C:\dmc.yaml");
                var deserializer = new Deserializer();
                Config = deserializer.Deserialize<Shared.Settings>(conf);
            }
            else
            {
                Config = new Shared.Settings();
            }

            if (Config.ComputerUnique == null) Config.ComputerUnique = Environment.MachineName;

            AdjustToken();

            var osi = Environment.OSVersion;
            OSInfo = new Shared.StatusReport.OSInfo()
            {
                Name = GetOSName(osi) + ' ' + osi.ServicePack,
                Version = osi.Version.ToString()
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        private static string GetOSName(OperatingSystem os)
        {
            if (os.Platform == PlatformID.Win32NT && os.Version.Major == 10 && os.Version.Minor == 0)
                return "Windows 10 / Server 2016 / Server 2019";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 3)
                return "Windows 8.1 / Server 2012 R2";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
                return "Windows 8 / Server 2012";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 1)
                return "Windows 7 / Server 2008 R2 / Home Server 2011";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 0)
                return "Windows Vista / Server 2008";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 5 && os.Version.Minor == 2)
                return "Windows XP 64-bit Edition / Server 2003 / Server 2003 R2 / Home Server";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 5 && os.Version.Minor == 1)
                return "Windows XP / Windows Fundamentals for Legacy PCs";
            else if (os.Platform == PlatformID.Win32Windows && os.Version.Major == 4 && os.Version.Minor == 90)
                return "Windows Millennium Edition";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 5 && os.Version.Minor == 0)
                return "Windows 2000";
            else if (os.Platform == PlatformID.Win32Windows && os.Version.Major == 4 && os.Version.Minor == 10)
                return "Windows 98 / 98 Second Edition";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 4 && os.Version.Minor == 0)
                return "Windows NT 4.0";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 3 && os.Version.Minor == 51)
                return "Windows NT 3.51";
            else if (os.Platform == PlatformID.Win32Windows && os.Version.Major == 4 && os.Version.Minor == 0)
                return "Windows 95";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 3 && os.Version.Minor == 5)
                return "Windows NT 3.5";
            else if (os.Platform == PlatformID.Win32NT && os.Version.Major == 3 && os.Version.Minor == 1)
                return "Windows NT 3.1";
            else
                return $"Unknown {os.Platform}";
        }
    }
}