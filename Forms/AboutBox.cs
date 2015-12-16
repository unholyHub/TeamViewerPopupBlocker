//-----------------------------------------------------------------------
// <copyright file="AboutBox.cs" company="Zhivko Kabaivanov">
//     Copyright (c) Zhivko Kabaivanov. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Forms
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Windows.Forms;
    using Classes;
    using Properties;
    using Settings = Classes.Settings;

    /// <summary>
    /// Partial class for showing the <see cref="AboutBox"/>.
    /// </summary>
    public partial class AboutBox : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutBox"/> class.
        /// </summary>
        public AboutBox()
        {
            this.InitializeComponent();
            this.Text = AssemblyTitle;
            this.labelProductName.Text = AssemblyProduct;

            this.labelVersion.Text = string.Format(CultureInfo.InvariantCulture, Resources.AboutBox_AboutBox_Version, Settings.Instance.AssemblyVersion);

            this.labelCopyright.Text = AssemblyCopyright;
            this.textBoxDescription.Text = AssemblyDescription + Resources.AboutBox_Description;
        }

        /// <summary>
        /// Gets the assembly product name information.
        /// </summary>
        public static string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }   

        /// <summary>
        /// Overriding the base class Text property.
        /// </summary>
        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Gets copyright information.
        /// </summary>
        private static string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets the application title.
        /// </summary>
        private static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];

                    if (!string.IsNullOrEmpty(titleAttribute.Title))
                    {
                        return titleAttribute.Title;
                    }
                }

                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// Gets the assembly description information.
        /// </summary>
        private static string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                if (attributes.Length == 0)
                {
                    return string.Empty;
                }

                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// When the ESC key is press, the window is closed.
        /// </summary>
        /// <param name="msg">The <see cref="Message"/> message.</param>
        /// <param name="keyData">The <see cref="Keys"/> key.</param>
        /// <returns>true if its pressed.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
            }

            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }

        /// <summary>
        /// Click event for donate button.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void DonateBtnClick(object sender, EventArgs e)
        {
            Process.Start(Settings.Instance.PayPalUrl.OriginalString);
        }

        /// <summary>
        /// Mouse enter event for donate button.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void DonateBtnMouseEnter(object sender, EventArgs e)
        {
            this.btnDonate.Image = Properties.Resources.donate_button_hover;
        }

        /// <summary>
        /// Mouse leave event for donate button
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void DonateBtnMouseLeave(object sender, EventArgs e)
        {
            this.btnDonate.Image = Properties.Resources.donate_button_normal;
        }

        /// <summary>
        /// Click event for donate button.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void AboutBox_Load(object sender, EventArgs e)
        {
            this.SetControlToolTippText();
        }

        /// <summary>
        /// Setting the control tooltip text descriptions.
        /// </summary>
        private void SetControlToolTippText()
        {
            this.ttHover.SetToolTip(this.btnDonate, Resources.AboutBox_DonateBtn_ToolTip);
            this.ttHover.SetToolTip(this.lblEmail, Resources.AboutBox_AboutBox_Load_Mailto);
            this.ttHover.SetToolTip(this.logoPictureBox, Resources.AboutBox_PicBox_ToolTip);
            this.copyEmailToolStripMenuItem.ToolTipText = Resources.AboutBox_SetControlToolTippText_Copy_email_to_clipboard;
        }

        /// <summary>
        /// Click event for <see cref="lblEmail"/> to show the <see cref="ctxmsEmail"/> or start the email client.
        /// </summary>
        /// /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void EmailLinkLblClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        this.ctxmsEmail.Show();
                        break;
                    }

                case MouseButtons.Left:
                    {
                        Process.Start("mailto:" + this.lblEmail.Text);
                        break;
                    }

                case MouseButtons.None:
                case MouseButtons.Middle:
                case MouseButtons.XButton1:
                case MouseButtons.XButton2:
                    {
                        break;
                    }

                default:
                    {
                        LogSystem.Instance.AddToLog(new ArgumentOutOfRangeException(Resources.AboutBox_EmailLinkLblClick_Unrecognized_mouse_button_is_clicked_), false);
                        break;
                    }
            }
        }

        /// <summary>
        /// Copy email to <see cref="Clipboard"/>.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void CopyEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.lblEmail.Text);
        }

        /// <summary>
        /// Click event for the picture box to open the blog on default browser.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> e.</param>
        private void LogoPbxClick(object sender, EventArgs e)
        {
            Process.Start(@"https://zhivkosk.wordpress.com/");
        }
    }
}
