using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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


        public Main()
        {
            InitializeComponent();

            timer_statusreport.Interval = Program.Config.Interval;
            timer_keepalive.Interval = Program.Config.KeepaliveInterval;
            timer_task.Interval = Program.Config.TaskInterval;

            Messenger = new Shared.Messenger(Program.Config.Server, Program.Config.Port, Program.Config.ComputerUnique);

            cpuPerc.NextValue();
            ramLeft.NextValue();
        }

        private async void Main_Shown(object sender, EventArgs e)
        {
            Hide();

            if (!await Messenger.CheckConnectionAsync())
            {
                MessageBox.Show("Connection failed. Tell Administrator.", "Device Management Console");

                Application.Exit();
            }
            timer_statusreport.Enabled = true;

            if (Program.Config.KeepaliveEnabled)
                timer_keepalive.Enabled = true;

            if (Program.Config.TaskEnabled)
                timer_task.Enabled = true;

            nicon_stbar.Text = "Device Management Console";

            lbl_connectionstatus.Text = $"Enabled ({Program.Config.ComputerUnique})";
            lbl_lastconnection.Text = "Server check: OK";
            lbl_kastatus.Text = "Keepalive: server checked";
            lbl_taskstatus.Text = "Remote Task: server checked";

            DenyStop = true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DenyStop)
                e.Cancel = true;
            Hide();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private async void timer_statusreport_Tick(object sender, EventArgs e)
        {
            var report = new Shared.StatusReport()
            {
                os = Program.OSInfo
            };


            report.processes = GetProcessInformations();
            report.services = GetServiceInformations();

            report.performance = new Shared.StatusReport.PerformanceInfo() {
                cpuPerc = cpuPerc.NextValue(),
                ramMb = ramLeft.NextValue(),
            };

            var json = System.Text.Json.JsonSerializer.Serialize(report, typeof(Shared.StatusReport));
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
                        commandLine = cmd,
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
                    status = stt.ToString(),
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