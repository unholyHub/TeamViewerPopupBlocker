using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using TeamViewerPopupBlocker.Classes;
using Settings = TeamViewerPopupBlocker.Classes.Settings;
using Timer = System.Windows.Forms.Timer;

namespace TeamViewerPopupBlocker.Forms
{
    public partial class MainForm : Form
    {
        private bool _allowClose;

        private readonly bool _allowVisible = false;

        private static AddWindowNameForm AddWindowNameForm { get; set; }

        private static AboutBox AboutBox { get; set; }

        private static bool IsRepeat { get; set; }

        private static Timer timer;

        public MainForm()
        {
            InitializeComponent();
            InitControls();

            Settings.Instance.Load();
            StartBlocking();
        }

        private void InitControls()
        {
            AddWindowNameForm = new AddWindowNameForm();
            AboutBox = new AboutBox();

            timer = new Timer();
            timer.Interval = 60 * 1000;
            timer.Tick += new EventHandler(timer1_Tick);
            ShowBallonTextToolTip($"Make {Settings.Instance.ProgramName} better!".ToString(CultureInfo.InvariantCulture));

            timer.Start();
        }

        private static void StopThread()
        {
            IsRepeat = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowBallonTextToolTip($"Make {Settings.Instance.ProgramName} better!");
        }

        private static void CloseWindow()
        {
            while (IsRepeat)
            {
                foreach (string popup in Settings.Instance.WindowNames)
                {
                    WinApi.CheckForWindowName(popup);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _allowClose = true;
            StopThread();

            this.Dispose();
            this.Close();

            Application.Exit();
        }

        private void StartBlockingMenuItemClick(object sender, EventArgs e)
        {
            StartBlocking();
        }

        private void StartBlocking()
        {
            try
            {
                Thread th = new Thread(CloseWindow) { Priority = ThreadPriority.Lowest};

                if (this.miStartBlocking.Enabled)
                {
                    this.miStopBlocking.Enabled = this.miStartBlocking.Enabled;
                    this.miStartBlocking.Enabled = !this.miStartBlocking.Enabled;
                }

                if (!th.IsAlive)
                {
                    IsRepeat = true;
                    th.Start();
                }

                Thread.Sleep(2000);
                ShowBallonTextToolTip("has started blocking.");
            }
            catch (ThreadStartException ex)
            {
                ShowBallonExceptionToolTip();
                LogSystem.Instance.AddToLog(ex);
            }
        }

        private void miStopBlocking_Click(object sender, EventArgs e)
        {
            StopBlocking();
        }

        private void StopBlocking()
        {
            try
            {
                if (this.miStopBlocking.Enabled)
                {
                    this.miStartBlocking.Enabled = this.miStopBlocking.Enabled;
                    this.miStopBlocking.Enabled = !this.miStopBlocking.Enabled;
                }

                StopThread();

                ShowBallonTextToolTip("has stopped blocking.");
            }
            catch (ThreadStateException ex)
            {
                ShowBallonExceptionToolTip();
                LogSystem.Instance.AddToLog(ex);
            }
        }

        private void AboutBoxMenuItemClick(object sender, EventArgs e)
        {
            OpenAboutBox();
        }

        private void OpenAboutBox()
        {
            if (AboutBox.Visible)
            {
                AboutBox.Focus();
                return;
            }

            using (AboutBox = new AboutBox())
            {
                AboutBox.ShowDialog(this);
            }
        }

        private void ShowBallonExceptionToolTip(int timeout = 3000)
        {
            this.niTray.ShowBalloonTip(timeout, Settings.Instance.ProgramName, "has thrown an exception", ToolTipIcon.Error);
        }

        private void ShowBallonTextToolTip(string tipText,int timeout = 3000, ToolTipIcon tipIcon = ToolTipIcon.Info)
        {
            this.niTray.ShowBalloonTip(timeout, Settings.Instance.ProgramName, tipText, tipIcon);
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            StopThread();
            Application.Exit();
        }

        private void TrayNiMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OpenAboutBox();
            }
        }

        private void miAddWindowName_Click(object sender, EventArgs e)
        {
            if (AddWindowNameForm.Visible)
            {
                AddWindowNameForm.Focus();
                return;
            }

            StopBlocking();
            
            try
            {
                AddWindowNameForm = new AddWindowNameForm();
                AddWindowNameForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                LogSystem.Instance.AddToLog(ex);
            }
            
            StartBlocking();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_allowClose)
            {
                this.Hide();
                if (e != null) e.Cancel = true;
            }

            base.OnFormClosing(e);
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!_allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) try
                {
                    CreateHandle();
                }
                catch (InvalidOperationException invalidOperationException)
                {
                        LogSystem.Instance.AddToLog(invalidOperationException);
                }
            }

            base.SetVisibleCore(value);
        }
    }
}