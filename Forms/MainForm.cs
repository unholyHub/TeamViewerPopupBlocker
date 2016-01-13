//-----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Zhivko Kabaivanov">
//     Copyright (c) Zhivko Kabaivanov. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------

using System.Drawing;
using TeamViewerPopupBlocker.Classes.Notifications;

namespace TeamViewerPopupBlocker.Forms
{
    using System;
    using System.Timers;
    using System.Windows.Forms;
    using Classes;
    using Properties;
    using Settings = Classes.Settings;
    using Timer = System.Windows.Forms.Timer;

    /// <summary>
    /// Partial class for showing the <see cref="AboutBox"/>.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The blocking <see cref="Timer"/> for closing the TeamViewer windows.
        /// </summary>
        private static Timer blockingTimer;

        /// <summary>
        /// The update <see cref="System.Timers.Timer"/> for notifying the user for new version.
        /// </summary>
        private static System.Timers.Timer updateTimer;

        /// <summary>
        /// Allowing the <see cref="MainForm"/> to be closed.
        /// </summary>
        private static bool allowClose;

        /// <summary>
        /// Allowing to the <see cref="MainForm"/> to be visible or not.
        /// </summary>
        private bool allowVisible = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.InitControls();

            UpdateNotifier.Instance.NotifyForUpdate();
            InitNotify();

            Settings.Instance.Load();
            this.StartBlocking();
        }

        /// <summary>
        /// Shows the notification message to the user.
        /// </summary>
        /// <param name="statusType"></param>
        /// <param name="srcStatusText"></param>
        /// <param name="srcStatusTimeOut"></param>
        public void ShowNotificationMessage(StatusType statusType, string srcStatusText, int srcStatusTimeOut)
        {
            switch (statusType)
            {
                case StatusType.StartBlocking:
                {
                    this.tbnRed.Hide();
                    this.tbnYellow.Hide();

                    this.tbnGreen.Show(string.Empty, srcStatusText, 300, srcStatusTimeOut, 50);

                    break;
                }

                case StatusType.StopBlocking:
                {
                    this.tbnGreen.Hide();
                    this.tbnYellow.Hide();

                    this.tbnRed.Show(string.Empty, srcStatusText, 300, srcStatusTimeOut, 50);
                    break;
                }

                case StatusType.InfoUpdate:
                case StatusType.InfoUpToDate:
                {
                    this.tbnGreen.Hide();
                    this.tbnRed.Hide();
                    this.tbnYellow.Show(string.Empty, srcStatusText, 300, srcStatusTimeOut, 50);

                    break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="AddWindowNameForm"/>.
        /// </summary>
        private static AddWindowNameForm AddWindowNameForm { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AboutBox"/>.
        /// </summary>
        private static AboutBox AboutBox { get; set; }

        /// <summary>
        /// Occurs before the form is closed.
        /// </summary>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> e.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            blockingTimer.Stop();

            if (!allowClose)
            {
                this.Hide();

                if (e != null)
                {
                    e.Cancel = true;
                }
            }

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Sets the control to the specified visible state.
        /// </summary>
        /// <param name="value"><see>
        ///         <cref>true</cref>
        ///     </see>
        ///     to make the control visible; otherwise, <see>
        ///         <cref>false</cref>
        ///     </see>
        /// .</param>
        protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;

                if (!this.IsHandleCreated)
                    try
                    {
                        CreateHandle();
                    }
                    catch (InvalidOperationException invalidOperationException)
                    {
                        LogSystem.Instance.AddToLog(invalidOperationException, false);
                    }
            }

            base.SetVisibleCore(value);
        }

        /// <summary>
        /// Initialization for the blocking <see cref="System.Timers.Timer"/>.
        /// </summary>
        private static void InitUpdateTimer()
        {
            updateTimer = new System.Timers.Timer();
            updateTimer.Interval = 24 * 60 * 60 * 1000; // 24 hours
            updateTimer.Elapsed += new ElapsedEventHandler(UpdateNotifyTimerElapsed);
            updateTimer.Start();
        }

        /// <summary>
        /// Notifying the user for a new update if available. 
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private static void UpdateNotifyTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateNotifier.Instance.NotifyForUpdate();
        }

        /// <summary>
        /// Initialization for the blocking <see cref="Timer"/>.
        /// </summary>
        private static void InitBlockingTimer()
        {
            blockingTimer = new Timer();
            blockingTimer.Interval = 200;
            blockingTimer.Tick += new EventHandler(BlockingTimerTick);
        }

        /// <summary>
        /// The blocking timer tick event.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private static void BlockingTimerTick(object sender, EventArgs e)
        {
            WindowOperations.CheckTeamViewerMainWindowTitle();
        }

        /// <summary>
        /// Initializes the necessary controls for the <see cref="MainForm"/>.
        /// </summary>
        private void InitControls()
        {
            AddWindowNameForm = new AddWindowNameForm();
            AboutBox = new AboutBox();

            InitBlockingTimer();
            InitUpdateTimer();
        }

        /// <summary>
        /// Click event for <see cref="MenuItem"/> for starting the blocking.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void StartBlockingMenuItemClick(object sender, EventArgs e)
        {
            this.StartBlocking();
        }

        /// <summary>
        /// Click event for <see cref="MenuItem"/> for stopping the blocking.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void StopBlockingMenuItemClick(object sender, EventArgs e)
        {
            this.StopBlocking();
        }

        /// <summary>
        /// Click event for <see cref="MenuItem"/> for displaying the <see cref="AddWindowNameForm"/> form.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void AddWindowNameMenuItemClick(object sender, EventArgs e)
        {
            if (AddWindowNameForm.Visible)
            {
                AddWindowNameForm.Focus();
                return;
            }

            this.StopBlocking();

            try
            {
                AddWindowNameForm = new AddWindowNameForm();
                AddWindowNameForm.ShowDialog(this);
            }
            catch (ArgumentNullException argumentNullException)
            {
                LogSystem.Instance.AddToLog(argumentNullException, false);
            }

            this.StartBlocking();
        }

        /// <summary>
        /// Click event for <see cref="MenuItem"/> for executing the <see cref="UpdateNotifier"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void UpdateCheckMenuItemClick(object sender, EventArgs e)
        {
            UpdateNotifier.Instance.NotifyForUpdate();
        }

        /// <summary>
        /// Click event for <see cref="MenuItem"/> for displaying the <see cref="AboutBox"/> form.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void AboutBoxMenuItemClick(object sender, EventArgs e)
        {
            this.OpenAboutBox();
        }

        /// <summary>
        /// Click event for <see cref="MenuItem"/> for exiting the application.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            allowClose = true;

            this.Dispose();

            Application.Exit();
        }

        /// <summary>
        /// Start the blocking.
        /// </summary>
        private void StartBlocking()
        {
            try
            {
                blockingTimer.Start();

                if (this.miStartBlocking.Enabled)
                {
                    this.miStopBlocking.Enabled = this.miStartBlocking.Enabled;
                    this.miStartBlocking.Enabled = !this.miStartBlocking.Enabled;
                }

                this.niTray.Icon = Resources.app_icon_default;
                ProgramStatus.Instance.AddStatus(new Status(StatusType.StartBlocking, 5000, 4000));
            }
            catch (ArgumentNullException ex)
            {
                this.ShowBallonExceptionToolTip();
                LogSystem.Instance.AddToLog(ex, false);
            }
        }

        /// <summary>
        /// Stop the blocking.
        /// </summary>
        private void StopBlocking()
        {
            try
            {
                blockingTimer.Stop();

                if (this.miStopBlocking.Enabled)
                {
                    this.miStartBlocking.Enabled = this.miStopBlocking.Enabled;
                    this.miStopBlocking.Enabled = !this.miStopBlocking.Enabled;
                }

                this.niTray.Icon = Resources.app_icon_turned_off;
                ProgramStatus.Instance.AddStatus(new Status(StatusType.StopBlocking, 5000, 4000));
            }
            catch (NullReferenceException ex)
            {
                ProgramStatus.Instance.AddStatus(new Status(StatusType.ErrorException, 5000, 4000));
                LogSystem.Instance.AddToLog(ex, false);
            }
        }

        /// <summary>
        /// Opening the <see cref="AboutBox"/>.
        /// </summary>
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

        /// <summary>
        /// Displays a balloon tip with the specified title, text, and icon in the taskbar for the specified time period.
        /// </summary>
        /// <param name="tipText">The text to display on the balloon tip.</param>
        /// <param name="timeout">The time period, in milliseconds, the balloon tip should display.</param>
        /// <param name="tipIcon">One of the <see cref="ToolTipIcon"/> values.</param>
        private void ShowBallonTextToolTip(string tipText, int timeout = 3000, ToolTipIcon tipIcon = ToolTipIcon.Info)
        {
            this.niTray.ShowBalloonTip(timeout, Resources.Program_Name, tipText, tipIcon);
        }

        /// <summary>
        /// Show exception balloon tooltip.
        /// </summary>
        /// <param name="timeout">The time period, in milliseconds, the balloon tip should display.</param>
        private void ShowBallonExceptionToolTip(int timeout = 3000)
        {
            this.niTray.ShowBalloonTip(timeout, Resources.Program_Name, "has thrown an exception", ToolTipIcon.Error);
        }

        /// <summary>
        /// Double mouse click event for showing the <see cref="AboutBox"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> e.</param>
        private void TrayNotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.OpenAboutBox();
            }
        }

        private TaskbarNotifier tbnGreen { get; set; }

        private TaskbarNotifier tbnRed { get; set; }

        private TaskbarNotifier tbnYellow { get; set; }

        private readonly Timer timerProgramStatus = new Timer();

        private void InitNotify()
        {
            tbnGreen = new TaskbarNotifier();
            tbnRed = new TaskbarNotifier();
            tbnYellow = new TaskbarNotifier();

            this.timerProgramStatus.Tick += new EventHandler(this.OnStatusCheck); //StatusProcessing
            this.timerProgramStatus.Interval = 100;
            this.timerProgramStatus.Start();

            InitNotify(this.tbnGreen, Resources.notification_green);
            InitNotify(this.tbnRed, Resources.notification_red);
            InitNotify(this.tbnYellow, Resources.notification_yellow);
        }

        private void InitNotify(TaskbarNotifier taskbarNotifier, Image inputImage)
        {
            taskbarNotifier.SetBackgroundBitmap(inputImage, Color.FromArgb(255, 0, 255));
            taskbarNotifier.SetCloseBitmap(
                Resources.close_buttons,
                Color.FromArgb(255, 0, 255),
                new Point(inputImage.Width - 40, 13));

            taskbarNotifier.ContentRectangle = new Rectangle(15, 30, inputImage.Width - 40, inputImage.Height - 50);
            taskbarNotifier.CloseClickable = true;
            taskbarNotifier.TitleClickable = false;
            taskbarNotifier.ContentClickable = true;
            taskbarNotifier.EnableSelectionRectangle = false;
            taskbarNotifier.KeepVisibleOnMousOver = true;
            taskbarNotifier.ReShowOnMouseOver = true;
        }

        private void OnStatusCheck(object source, EventArgs e)
        {
            ProgramStatus.Instance.GetStatus(this);
        }
    }
}