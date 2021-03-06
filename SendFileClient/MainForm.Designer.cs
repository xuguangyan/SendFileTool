﻿namespace SendFileClient
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.lbFinish = new System.Windows.Forms.Label();
            this.lbPath = new System.Windows.Forms.Label();
            this.lbSeconds = new System.Windows.Forms.Label();
            this.lbTongji = new System.Windows.Forms.Label();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.rdBtnTCP = new System.Windows.Forms.RadioButton();
            this.rdBtnUDP = new System.Windows.Forms.RadioButton();
            this.btnFileOpen = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnconnect = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lbFileOpen = new System.Windows.Forms.Label();
            this.lbIP = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.picBoxClean = new System.Windows.Forms.PictureBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.timerSendState = new System.Timers.Timer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerSaveCfg = new System.Timers.Timer();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rdBtnShutdown = new System.Windows.Forms.RadioButton();
            this.rdBtnExitApp = new System.Windows.Forms.RadioButton();
            this.rdBtnNothing = new System.Windows.Forms.RadioButton();
            this.lbComplete = new System.Windows.Forms.Label();
            this.chkShowLog = new System.Windows.Forms.CheckBox();
            this.trackBarInterval = new System.Windows.Forms.TrackBar();
            this.lbIntervalTime = new System.Windows.Forms.Label();
            this.lbMilliSecond = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.openFolderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.prgBar1 = new SendFileCommon.CustomProgressBar();
            this.ctxMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuItemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxClean)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerSendState)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timerSaveCfg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarInterval)).BeginInit();
            this.ctxMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpen);
            this.groupBox1.Controls.Add(this.btnPause);
            this.groupBox1.Controls.Add(this.lbFinish);
            this.groupBox1.Controls.Add(this.lbPath);
            this.groupBox1.Controls.Add(this.lbSeconds);
            this.groupBox1.Controls.Add(this.lbTongji);
            this.groupBox1.Controls.Add(this.btnSendMsg);
            this.groupBox1.Controls.Add(this.rdBtnTCP);
            this.groupBox1.Controls.Add(this.rdBtnUDP);
            this.groupBox1.Controls.Add(this.btnFileOpen);
            this.groupBox1.Controls.Add(this.btnSendFile);
            this.groupBox1.Controls.Add(this.btnconnect);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtFile);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Controls.Add(this.lbFileOpen);
            this.groupBox1.Controls.Add(this.lbIP);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 136);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器信息";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(430, 48);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(53, 23);
            this.btnOpen.TabIndex = 51;
            this.btnOpen.Text = "浏览";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(430, 19);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(53, 23);
            this.btnPause.TabIndex = 22;
            this.btnPause.Text = "暂停";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // lbFinish
            // 
            this.lbFinish.AutoSize = true;
            this.lbFinish.Location = new System.Drawing.Point(199, 108);
            this.lbFinish.Name = "lbFinish";
            this.lbFinish.Size = new System.Drawing.Size(77, 12);
            this.lbFinish.TabIndex = 21;
            this.lbFinish.Text = "完成：0/0 kb";
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Location = new System.Drawing.Point(15, 53);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(65, 12);
            this.lbPath.TabIndex = 49;
            this.lbPath.Text = "保存路径：";
            // 
            // lbSeconds
            // 
            this.lbSeconds.AutoSize = true;
            this.lbSeconds.ForeColor = System.Drawing.Color.Red;
            this.lbSeconds.Location = new System.Drawing.Point(84, 108);
            this.lbSeconds.Name = "lbSeconds";
            this.lbSeconds.Size = new System.Drawing.Size(0, 12);
            this.lbSeconds.TabIndex = 19;
            // 
            // lbTongji
            // 
            this.lbTongji.AutoSize = true;
            this.lbTongji.Location = new System.Drawing.Point(15, 108);
            this.lbTongji.Name = "lbTongji";
            this.lbTongji.Size = new System.Drawing.Size(65, 12);
            this.lbTongji.TabIndex = 17;
            this.lbTongji.Text = "发送耗时：";
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(415, 99);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(68, 31);
            this.btnSendMsg.TabIndex = 15;
            this.btnSendMsg.Text = "发送消息";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // rdBtnTCP
            // 
            this.rdBtnTCP.AutoSize = true;
            this.rdBtnTCP.Location = new System.Drawing.Point(386, 26);
            this.rdBtnTCP.Name = "rdBtnTCP";
            this.rdBtnTCP.Size = new System.Drawing.Size(41, 16);
            this.rdBtnTCP.TabIndex = 14;
            this.rdBtnTCP.Text = "TCP";
            this.rdBtnTCP.UseVisualStyleBackColor = true;
            // 
            // rdBtnUDP
            // 
            this.rdBtnUDP.AutoSize = true;
            this.rdBtnUDP.Checked = true;
            this.rdBtnUDP.Location = new System.Drawing.Point(332, 26);
            this.rdBtnUDP.Name = "rdBtnUDP";
            this.rdBtnUDP.Size = new System.Drawing.Size(41, 16);
            this.rdBtnUDP.TabIndex = 13;
            this.rdBtnUDP.TabStop = true;
            this.rdBtnUDP.Text = "UDP";
            this.rdBtnUDP.UseVisualStyleBackColor = true;
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.Location = new System.Drawing.Point(430, 72);
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(53, 23);
            this.btnFileOpen.TabIndex = 11;
            this.btnFileOpen.Text = "浏览";
            this.btnFileOpen.UseVisualStyleBackColor = true;
            this.btnFileOpen.Click += new System.EventHandler(this.btnFileOpen_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(341, 99);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(68, 31);
            this.btnSendFile.TabIndex = 10;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnconnect
            // 
            this.btnconnect.Location = new System.Drawing.Point(251, 15);
            this.btnconnect.Name = "btnconnect";
            this.btnconnect.Size = new System.Drawing.Size(68, 31);
            this.btnconnect.TabIndex = 9;
            this.btnconnect.Text = "连  接";
            this.btnconnect.UseVisualStyleBackColor = true;
            this.btnconnect.Click += new System.EventHandler(this.btnconnect_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(201, 25);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(44, 21);
            this.txtPort.TabIndex = 8;
            this.txtPort.Text = "8001";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(86, 72);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(338, 21);
            this.txtFile.TabIndex = 7;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(86, 25);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 21);
            this.txtIP.TabIndex = 7;
            this.txtIP.Text = "127.0.0.1";
            // 
            // lbFileOpen
            // 
            this.lbFileOpen.AutoSize = true;
            this.lbFileOpen.Location = new System.Drawing.Point(15, 81);
            this.lbFileOpen.Name = "lbFileOpen";
            this.lbFileOpen.Size = new System.Drawing.Size(65, 12);
            this.lbFileOpen.TabIndex = 6;
            this.lbFileOpen.Text = "浏览文件：";
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(26, 30);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(53, 12);
            this.lbIP.TabIndex = 5;
            this.lbIP.Text = "绑定IP：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.picBoxClean);
            this.groupBox3.Controls.Add(this.txtStatus);
            this.groupBox3.Location = new System.Drawing.Point(10, 193);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(494, 129);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "状态信息";
            // 
            // picBoxClean
            // 
            this.picBoxClean.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBoxClean.Location = new System.Drawing.Point(63, -3);
            this.picBoxClean.Name = "picBoxClean";
            this.picBoxClean.Size = new System.Drawing.Size(18, 18);
            this.picBoxClean.TabIndex = 41;
            this.picBoxClean.TabStop = false;
            this.toolTip1.SetToolTip(this.picBoxClean, "清除状态信息");
            this.picBoxClean.Click += new System.EventHandler(this.picBoxClean_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(6, 19);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(482, 104);
            this.txtStatus.TabIndex = 14;
            // 
            // openFileDlg
            // 
            this.openFileDlg.FileName = "openFileDialog1";
            // 
            // timerSendState
            // 
            this.timerSendState.SynchronizingObject = this;
            this.timerSendState.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSendState_Elapsed);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 325);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(516, 22);
            this.statusStrip1.TabIndex = 40;
            // 
            // tssLabel
            // 
            this.tssLabel.Name = "tssLabel";
            this.tssLabel.Size = new System.Drawing.Size(68, 17);
            this.tssLabel.Text = "传输速度：";
            // 
            // timerSaveCfg
            // 
            this.timerSaveCfg.Interval = 60000D;
            this.timerSaveCfg.SynchronizingObject = this;
            this.timerSaveCfg.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSaveCfg_Elapsed);
            // 
            // rdBtnShutdown
            // 
            this.rdBtnShutdown.AutoSize = true;
            this.rdBtnShutdown.Location = new System.Drawing.Point(421, 178);
            this.rdBtnShutdown.Name = "rdBtnShutdown";
            this.rdBtnShutdown.Size = new System.Drawing.Size(71, 16);
            this.rdBtnShutdown.TabIndex = 43;
            this.rdBtnShutdown.Text = "关闭电脑";
            this.rdBtnShutdown.UseVisualStyleBackColor = true;
            // 
            // rdBtnExitApp
            // 
            this.rdBtnExitApp.AutoSize = true;
            this.rdBtnExitApp.Location = new System.Drawing.Point(352, 178);
            this.rdBtnExitApp.Name = "rdBtnExitApp";
            this.rdBtnExitApp.Size = new System.Drawing.Size(71, 16);
            this.rdBtnExitApp.TabIndex = 44;
            this.rdBtnExitApp.Text = "关闭程序";
            this.rdBtnExitApp.UseVisualStyleBackColor = true;
            // 
            // rdBtnNothing
            // 
            this.rdBtnNothing.AutoSize = true;
            this.rdBtnNothing.Checked = true;
            this.rdBtnNothing.Location = new System.Drawing.Point(295, 178);
            this.rdBtnNothing.Name = "rdBtnNothing";
            this.rdBtnNothing.Size = new System.Drawing.Size(59, 16);
            this.rdBtnNothing.TabIndex = 45;
            this.rdBtnNothing.TabStop = true;
            this.rdBtnNothing.Text = "不操作";
            this.rdBtnNothing.UseVisualStyleBackColor = true;
            // 
            // lbComplete
            // 
            this.lbComplete.AutoSize = true;
            this.lbComplete.Location = new System.Drawing.Point(232, 180);
            this.lbComplete.Name = "lbComplete";
            this.lbComplete.Size = new System.Drawing.Size(65, 12);
            this.lbComplete.TabIndex = 42;
            this.lbComplete.Text = "发送完成：";
            // 
            // chkShowLog
            // 
            this.chkShowLog.AutoSize = true;
            this.chkShowLog.Checked = true;
            this.chkShowLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLog.Location = new System.Drawing.Point(149, 178);
            this.chkShowLog.Name = "chkShowLog";
            this.chkShowLog.Size = new System.Drawing.Size(84, 16);
            this.chkShowLog.TabIndex = 41;
            this.chkShowLog.Text = "显示流日志";
            this.chkShowLog.UseVisualStyleBackColor = true;
            // 
            // trackBarInterval
            // 
            this.trackBarInterval.LargeChange = 50;
            this.trackBarInterval.Location = new System.Drawing.Point(255, 326);
            this.trackBarInterval.Maximum = 1000;
            this.trackBarInterval.Name = "trackBarInterval";
            this.trackBarInterval.Size = new System.Drawing.Size(200, 45);
            this.trackBarInterval.SmallChange = 50;
            this.trackBarInterval.TabIndex = 46;
            this.trackBarInterval.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarInterval.Scroll += new System.EventHandler(this.trackBarInterval_Scroll);
            // 
            // lbIntervalTime
            // 
            this.lbIntervalTime.AutoSize = true;
            this.lbIntervalTime.Location = new System.Drawing.Point(197, 331);
            this.lbIntervalTime.Name = "lbIntervalTime";
            this.lbIntervalTime.Size = new System.Drawing.Size(65, 12);
            this.lbIntervalTime.TabIndex = 47;
            this.lbIntervalTime.Text = "间歇时间：";
            // 
            // lbMilliSecond
            // 
            this.lbMilliSecond.AutoSize = true;
            this.lbMilliSecond.Location = new System.Drawing.Point(455, 331);
            this.lbMilliSecond.Name = "lbMilliSecond";
            this.lbMilliSecond.Size = new System.Drawing.Size(29, 12);
            this.lbMilliSecond.TabIndex = 48;
            this.lbMilliSecond.Text = "0 ms";
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(98, 60);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(338, 21);
            this.txtFolder.TabIndex = 50;
            this.txtFolder.Text = "D:\\";
            // 
            // prgBar1
            // 
            this.prgBar1.Location = new System.Drawing.Point(10, 154);
            this.prgBar1.Name = "prgBar1";
            this.prgBar1.Size = new System.Drawing.Size(494, 18);
            this.prgBar1.TabIndex = 51;
            this.prgBar1.Text = null;
            this.prgBar1.TextColor = System.Drawing.Color.Black;
            this.prgBar1.TextFont = new System.Drawing.Font("宋体", 9F);
            // 
            // ctxMenuStrip
            // 
            this.ctxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShow,
            this.menuItemAbout,
            this.menuItemExit});
            this.ctxMenuStrip.Name = "ctxMenuStrip";
            this.ctxMenuStrip.Size = new System.Drawing.Size(142, 70);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.ctxMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "文件发送-客户端";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // menuItemShow
            // 
            this.menuItemShow.Name = "menuItemShow";
            this.menuItemShow.Size = new System.Drawing.Size(141, 22);
            this.menuItemShow.Text = "隐藏界面(&H)";
            this.menuItemShow.Click += new System.EventHandler(this.menuItemShow_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(141, 22);
            this.menuItemExit.Text = "退出程序(&X)";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(141, 22);
            this.menuItemAbout.Text = "关于软件(&A)";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 347);
            this.Controls.Add(this.prgBar1);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.lbMilliSecond);
            this.Controls.Add(this.lbIntervalTime);
            this.Controls.Add(this.trackBarInterval);
            this.Controls.Add(this.rdBtnShutdown);
            this.Controls.Add(this.rdBtnExitApp);
            this.Controls.Add(this.rdBtnNothing);
            this.Controls.Add(this.lbComplete);
            this.Controls.Add(this.chkShowLog);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件发送-客户端(大圣制作)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.MainForm_DragOver);
            this.DragLeave += new System.EventHandler(this.MainForm_DragLeave);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxClean)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerSendState)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timerSaveCfg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarInterval)).EndInit();
            this.ctxMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnconnect;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label lbFileOpen;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnFileOpen;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.RadioButton rdBtnTCP;
        private System.Windows.Forms.RadioButton rdBtnUDP;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.Label lbSeconds;
        private System.Windows.Forms.Label lbTongji;
        private System.Timers.Timer timerSendState;
        private System.Windows.Forms.Label lbFinish;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssLabel;
        private System.Windows.Forms.Button btnPause;
        private System.Timers.Timer timerSaveCfg;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox picBoxClean;
        private System.Windows.Forms.RadioButton rdBtnShutdown;
        private System.Windows.Forms.RadioButton rdBtnExitApp;
        private System.Windows.Forms.RadioButton rdBtnNothing;
        private System.Windows.Forms.Label lbComplete;
        private System.Windows.Forms.CheckBox chkShowLog;
        private System.Windows.Forms.TrackBar trackBarInterval;
        private System.Windows.Forms.Label lbMilliSecond;
        private System.Windows.Forms.Label lbIntervalTime;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label lbPath;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.FolderBrowserDialog openFolderDlg;
        private SendFileCommon.CustomProgressBar prgBar1;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemShow;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
    }
}

