namespace DeviceManagementConsole.WindowsClient
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lbl_connectionstatus = new System.Windows.Forms.Label();
            this.lbl_lastconnection = new System.Windows.Forms.Label();
            this.timer_statusreport = new System.Windows.Forms.Timer(this.components);
            this.timer_keepalive = new System.Windows.Forms.Timer(this.components);
            this.nicon_stbar = new System.Windows.Forms.NotifyIcon(this.components);
            this.lbl_kastatus = new System.Windows.Forms.Label();
            this.timer_task = new System.Windows.Forms.Timer(this.components);
            this.lbl_taskstatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_connectionstatus
            // 
            this.lbl_connectionstatus.AutoSize = true;
            this.lbl_connectionstatus.Location = new System.Drawing.Point(12, 9);
            this.lbl_connectionstatus.Name = "lbl_connectionstatus";
            this.lbl_connectionstatus.Size = new System.Drawing.Size(103, 12);
            this.lbl_connectionstatus.TabIndex = 0;
            this.lbl_connectionstatus.Text = "Getting Information";
            // 
            // lbl_lastconnection
            // 
            this.lbl_lastconnection.AutoSize = true;
            this.lbl_lastconnection.Location = new System.Drawing.Point(12, 32);
            this.lbl_lastconnection.Name = "lbl_lastconnection";
            this.lbl_lastconnection.Size = new System.Drawing.Size(88, 12);
            this.lbl_lastconnection.TabIndex = 1;
            this.lbl_lastconnection.Text = "Waiting Request";
            // 
            // timer_statusreport
            // 
            this.timer_statusreport.Tick += new System.EventHandler(this.timer_statusreport_Tick);
            // 
            // timer_keepalive
            // 
            this.timer_keepalive.Tick += new System.EventHandler(this.timer_keepalive_Tick);
            // 
            // nicon_stbar
            // 
            this.nicon_stbar.BalloonTipText = "Device Management Console";
            this.nicon_stbar.BalloonTipTitle = "Message from Administrator.";
            this.nicon_stbar.Icon = ((System.Drawing.Icon)(resources.GetObject("nicon_stbar.Icon")));
            this.nicon_stbar.Text = "Booting - Device Management Console";
            this.nicon_stbar.Visible = true;
            this.nicon_stbar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.nicon_stbar_MouseDoubleClick);
            // 
            // lbl_kastatus
            // 
            this.lbl_kastatus.AutoSize = true;
            this.lbl_kastatus.Location = new System.Drawing.Point(12, 44);
            this.lbl_kastatus.Name = "lbl_kastatus";
            this.lbl_kastatus.Size = new System.Drawing.Size(88, 12);
            this.lbl_kastatus.TabIndex = 2;
            this.lbl_kastatus.Text = "Waiting Request";
            // 
            // timer_task
            // 
            this.timer_task.Tick += new System.EventHandler(this.timer_task_Tick);
            // 
            // lbl_taskstatus
            // 
            this.lbl_taskstatus.AutoSize = true;
            this.lbl_taskstatus.Location = new System.Drawing.Point(12, 56);
            this.lbl_taskstatus.Name = "lbl_taskstatus";
            this.lbl_taskstatus.Size = new System.Drawing.Size(88, 12);
            this.lbl_taskstatus.TabIndex = 3;
            this.lbl_taskstatus.Text = "Waiting Request";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 122);
            this.Controls.Add(this.lbl_taskstatus);
            this.Controls.Add(this.lbl_kastatus);
            this.Controls.Add(this.lbl_lastconnection);
            this.Controls.Add(this.lbl_connectionstatus);
            this.Name = "Main";
            this.Text = "Device Management Console - Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_connectionstatus;
        private System.Windows.Forms.Label lbl_lastconnection;
        private System.Windows.Forms.Timer timer_statusreport;
        private System.Windows.Forms.Timer timer_keepalive;
        private System.Windows.Forms.NotifyIcon nicon_stbar;
        private System.Windows.Forms.Label lbl_kastatus;
        private System.Windows.Forms.Timer timer_task;
        private System.Windows.Forms.Label lbl_taskstatus;
    }
}

