using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceManagementConsole.WindowsClient
{
    public partial class Main : Form
    {
        private Shared.Messenger Messenger;

        internal static bool DenyStop = false;

        private PerformanceCounter cpuPerc = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter ramLeft = new PerformanceCounter("Memory", "Available MBytes");
        private PerformanceCounter upTime = new PerformanceCounter("System", "System Up Time");

        private int Status = 0;

        public Main()
        {
            InitializeComponent();

            timer_statusreport.Interval = Program.Config.Interval;
            timer_keepalive.Interval = Program.Config.KeepaliveInterval;
            timer_task.Interval = Program.Config.TaskInterval;

            Messenger = new Shared.Messenger(Program.Config.Server, Program.Config.Port, Program.Config.ComputerUnique);

            cpuPerc.NextValue();
            ramLeft.NextValue();
            upTime.NextValue();
        }

        private async void Main_Shown(object sender, EventArgs e)
        {
            Program.ShutdownBlockReasonCreate(Handle, "Sending shutdown signal");

            Hide();

            if (!await Messenger.CheckConnectionAsync())
            {
                MessageBox.Show("Connection failed. Tell Administrator.", "Device Management Console");

                Application.Exit();
            }
            timer_statusreport.Enabled = true;

            if (Program.Config.KeepaliveEnabled)
            {
                timer_keepalive.Enabled = true;
                lbl_kastatus.Text = "Keepalive: waiting for connect";
                timer_keepalive_Tick(null, null);
            }
            else
            {
                lbl_kastatus.Text = "Keepalive: disabled";
            }

            if (Program.Config.TaskEnabled)
            {
                timer_task.Enabled = true;
                lbl_taskstatus.Text = "Remote Task: waiting for connect";
                timer_task_Tick(null, null);
            }
            else
            {
                lbl_taskstatus.Text = "Remote Task: disabled";
            }

            nicon_stbar.Text = "Device Management Console";

            lbl_connectionstatus.Text = $"Enabled ({Program.Config.ComputerUnique})";
            lbl_lastconnection.Text = "Server check: OK";
            timer_statusreport_Tick(null, null);

            DenyStop = true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                ReportStatusASAP(2);
            }
            else if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                ReportStatusASAP(3);
            }
            else if (e.CloseReason == CloseReason.UserClosing && DenyStop)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                ReportStatusASAP(1);
                e.Cancel = true;
            }
        }

        private void ReportStatusASAP(int status = -1)
        {
            var report = new Shared.StatusReport()
            {
                ClientVersion = Program.Version,
                Status = Status = status,
                OS = Program.OSInfo,
            };

            var json = JsonSerializer.Serialize(report, typeof(Shared.StatusReport), Shared.Messenger.JsonSerializerOptions);
            Messenger.SendReportJson(json);
            Debug.WriteLine("Sent ASAP Report");
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine(e.CloseReason);
        }

        private async void timer_statusreport_Tick(object sender, EventArgs e)
        {
            var report = new Shared.StatusReport()
            {
                ClientVersion = Program.Version,
                Status = Status,
                OS = Program.OSInfo,
            };

            report.Processes = GetProcessInformations();
            report.Services = GetServiceInformations();

            report.Performance = new Shared.StatusReport.PerformanceInfo()
            {
                CpuPerc = cpuPerc.NextValue(),
                RamMb = ramLeft.NextValue(),
                Uptime = (ulong)upTime.NextValue(),
            };

            var json = JsonSerializer.Serialize(report, typeof(Shared.StatusReport), Shared.Messenger.JsonSerializerOptions);
            bool result = await Task.Run(() => Messenger.SendReportJsonAsync(json));

            if (result)
                lbl_lastconnection.Text = "Status report: reported";
            else
                lbl_lastconnection.Text = "Status report: failed";
        }

        private static IEnumerable<Shared.StatusReport.ProcessInfo> GetProcessInformations()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process"))
            using (var procs = searcher.Get())
            {
                foreach (ManagementObject proc in procs)
                {
                    uint id = (uint)proc["ProcessId"];
                    string name = proc["Name"]?.ToString();
                    string cmd = proc["CommandLine"]?.ToString();

                    var i = new Shared.StatusReport.ProcessInfo((int)id, name)
                    {
                        CommandLine = cmd,
                    };

                    yield return i;
                }
            }
        }

        private static IEnumerable<Shared.StatusReport.ServiceInfo> GetServiceInformations()
        {
            var servs = ServiceController.GetServices();
            foreach (var serv in servs)
            {
                var stt = serv.Status;
                var i = new Shared.StatusReport.ServiceInfo(serv.DisplayName)
                {
                    Status = stt.ToString(),
                };
                yield return i;
            }
        }

        private async void timer_keepalive_Tick(object sender, EventArgs e)
        {
            bool result = await Task.Run(() => Messenger.SendKeepaliveAsync());

            if (result)
                lbl_kastatus.Text = "Keepalive: reported";
            else
                lbl_kastatus.Text = "Keepalive: failed";
        }

        private void nicon_stbar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }

        private async void timer_task_Tick(object sender, EventArgs e)
        {
            string json = await Messenger.GetRemoteTaskJsonAsync();
            if (json == null)
            {
                lbl_taskstatus.Text = "Remote Task: no task";
                return;
            }

            var task = JsonSerializer.Deserialize<Shared.RemoteTask>(json);

            var result = Tasking.RunTask(task);

            if (result)
                lbl_taskstatus.Text = "Remote Task: task executed";
            else
                lbl_taskstatus.Text = "Remote Task: task failed";
        }
    }
}