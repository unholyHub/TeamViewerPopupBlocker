using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TeamViewerPopupBlocker
{
    public partial class MainForm : Form
    {
        private static string[] popups = new[] { "Commercial use", "Sponsored session", "Commercial use suspected", "Unable to connect" };

        private const uint WM_CLOSE = 0x0010;

        public MainForm()
        {
            InitializeComponent();
            StartBlocking();
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, int childAfter, string lclassName, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        // Define the FindWindow API function.

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        private static bool IsRepeat { get; set; }

        private const int BN_CLICKED = 245;

        private static void CloseWindow()
        {
            while (IsRepeat)
            {
                foreach (string popup in popups)
                {
                    IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, popup);

                    if (windowPtr != IntPtr.Zero)
                    {
                        SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                        var butn = FindWindowEx(windowPtr, 0, "Button", "OK");

                        if (butn != IntPtr.Zero)
                        {
                            SendMessage(butn, BN_CLICKED, IntPtr.Zero, IntPtr.Zero);
                        }
                    }
                }
            }
        }

        private bool allowVisible; // ContextMenu's Show command used
        private bool allowClose; // ContextMenu's Exit command used

        protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!allowClose)
            {
                this.Hide();
                e.Cancel = true;
            }

            base.OnFormClosing(e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allowClose = true;
            StopThread();
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
                Thread th = new Thread(CloseWindow) { Priority = ThreadPriority.Lowest };

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

                ShowBallonToolTip("has started blocking.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void miStopBlocking_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.miStopBlocking.Enabled)
                {
                    this.miStartBlocking.Enabled = this.miStopBlocking.Enabled;
                    this.miStopBlocking.Enabled = !this.miStopBlocking.Enabled;
                }

                StopThread();

                ShowBallonToolTip("has stopped blocking.");
            }
            catch { }
        }

        private static void StopThread()
        {
            IsRepeat = false;
            //if (th.IsAlive)
            //{
            //    th.Abort();
            //}
        }

        private void AboutBoxMenuItemClick(object sender, EventArgs e)
        {
            using (AboutBox box = new AboutBox())
            {
                box.ShowDialog(this);
            }
        }

        private void ShowBallonToolTip(string ballonTipText)
        {
            //this.mynotifyicon.Icon = SystemIcons.Information;
            this.mynotifyicon.BalloonTipTitle = "TeamViewer Popup Blocker";
            this.mynotifyicon.BalloonTipText = ballonTipText;
            this.mynotifyicon.ShowBalloonTip(2000);

            //MessageBox.Show(ballonTipText);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopThread();
        }
    }
}