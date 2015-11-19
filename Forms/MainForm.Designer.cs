using TeamViewerPopupBlocker.Properties;

namespace TeamViewerPopupBlocker.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miStartBlocking = new System.Windows.Forms.ToolStripMenuItem();
            this.miStopBlocking = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miAddWindowName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // niTray
            // 
            this.niTray.ContextMenuStrip = this.contextMenuStrip;
            this.niTray.Icon = ((System.Drawing.Icon)(resources.GetObject("niTray.Icon")));
            this.niTray.Text = global::TeamViewerPopupBlocker.Properties.Resources.Settings_ProgramName;
            this.niTray.Visible = true;
            this.niTray.BalloonTipClicked += new System.EventHandler(this.TrayNiBalloonTipClicked);
            this.niTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayNiMouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miStartBlocking,
            this.miStopBlocking,
            this.toolStripSeparator1,
            this.miAddWindowName,
            this.toolStripSeparator2,
            this.miAbout,
            this.toolStripSeparator3,
            this.miExit});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(179, 132);
            // 
            // miStartBlocking
            // 
            this.miStartBlocking.Image = global::TeamViewerPopupBlocker.Properties.Resources.start;
            this.miStartBlocking.Name = "miStartBlocking";
            this.miStartBlocking.Size = new System.Drawing.Size(178, 22);
            this.miStartBlocking.Text = global::TeamViewerPopupBlocker.Properties.Resources.MainForm_InitializeComponent_Start_Blocking;
            this.miStartBlocking.Click += new System.EventHandler(this.StartBlockingMenuItemClick);
            // 
            // miStopBlocking
            // 
            this.miStopBlocking.Image = global::TeamViewerPopupBlocker.Properties.Resources.stop;
            this.miStopBlocking.Name = "miStopBlocking";
            this.miStopBlocking.Size = new System.Drawing.Size(178, 22);
            this.miStopBlocking.Text = global::TeamViewerPopupBlocker.Properties.Resources.MainForm_InitializeComponent_Stop_Blocking;
            this.miStopBlocking.Click += new System.EventHandler(this.miStopBlocking_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // miAddWindowName
            // 
            this.miAddWindowName.Image = global::TeamViewerPopupBlocker.Properties.Resources.add_window_name;
            this.miAddWindowName.Name = "miAddWindowName";
            this.miAddWindowName.Size = new System.Drawing.Size(178, 22);
            this.miAddWindowName.Text = global::TeamViewerPopupBlocker.Properties.Resources.MainForm_InitializeComponent_Add_Window_Name;
            this.miAddWindowName.Click += new System.EventHandler(this.miAddWindowName_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
            // 
            // miAbout
            // 
            this.miAbout.Image = global::TeamViewerPopupBlocker.Properties.Resources.about;
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(178, 22);
            this.miAbout.Text = global::TeamViewerPopupBlocker.Properties.Resources.MainForm_InitializeComponent_About;
            this.miAbout.Click += new System.EventHandler(this.AboutBoxMenuItemClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(175, 6);
            // 
            // miExit
            // 
            this.miExit.Image = global::TeamViewerPopupBlocker.Properties.Resources.exit;
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(178, 22);
            this.miExit.Text = global::TeamViewerPopupBlocker.Properties.Resources.MainForm_InitializeComponent_Exit;
            this.miExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 0);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "TeamViewer Popup Blocker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon niTray;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miStartBlocking;
        private System.Windows.Forms.ToolStripMenuItem miStopBlocking;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miAddWindowName;
    }
}
