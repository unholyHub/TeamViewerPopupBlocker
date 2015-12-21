//-----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Zhivko Kabaivanov">
//     Copyright (c) Zhivko Kabaivanov. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Forms
{
    using System;
    using System.Globalization;
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
        /// The promotion <see cref="Timer"/> for showing the donation popup balloon. 
        /// </summary>
        private static Timer donationTimer;

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

            UpdateNotifier.Instance.NotifyForUpdate(true);

            Settings.Instance.Load();
            this.StartBlocking();
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
            if(!allowVisible)
            {
                value = false;

                if (!this.IsHandleCreated) try
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
        /// Initialization for the blocking <see cref="Timer"/>.
        /// </summary>
        private static void InitBlockingTimer()
        {
            blockingTimer = new Timer();
            blockingTimer.Interval = 1000;
            blockingTimer.Tick += new EventHandler(BlockingTimerTick);
        }
        
        /// <summary>
        /// Notifying the user for a new update if available. 
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private static void UpdateNotifyTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateNotifier.Instance.NotifyForUpdate(false);
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

            this.InitPromotionTimer();
            InitBlockingTimer();
            InitUpdateTimer();

            this.ShowBallonTextToolTip(string.Format(CultureInfo.InvariantCulture, "Make {0} better!", Resources.Program_Name));

            donationTimer.Start();
        }

        /// <summary>
        /// Initialization for the promotion <see cref="Timer"/>.
        /// </summary>
        private void InitPromotionTimer()
        {
            donationTimer = new Timer();
            donationTimer.Interval = 60 * 60 * 1000;  // 10 min.
            donationTimer.Tick += new EventHandler(this.PromotionTimerTick);
        }
        
        /// <summary>
        /// The blocking timer tick event.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void PromotionTimerTick(object sender, EventArgs e)
        {
            this.ShowBallonTextToolTip(string.Format(CultureInfo.InvariantCulture, "Make {0} better!", Resources.Program_Name));
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
            UpdateNotifier.Instance.NotifyForUpdate(false);
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
                this.ShowBallonTextToolTip("has started blocking.");
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
                this.ShowBallonTextToolTip("has stopped blocking.");
            }
            catch (NullReferenceException ex)
            {
                this.ShowBallonExceptionToolTip();
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
    }
}