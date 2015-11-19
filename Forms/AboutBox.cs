using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using TeamViewerPopupBlocker.Properties;
using Settings = TeamViewerPopupBlocker.Classes.Settings;

namespace TeamViewerPopupBlocker.Forms
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = AssemblyTitle;
            this.labelProductName.Text = AssemblyProduct;

            this.labelVersion.Text = string.Format(CultureInfo.InvariantCulture,
                                                   Resources.AboutBox_AboutBox_Version, 
                                                   AssemblyVersion);

            this.labelCopyright.Text = AssemblyCopyright;
            this.textBoxDescription.Text = AssemblyDescription + Resources.AboutBox_Description;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #region Assembly Attribute Accessors

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

        private static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }

        #endregion

        private void lblEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            Process.Start(Settings.Instance.PayPalUrl);
        }

        private void btnDonate_MouseEnter(object sender, EventArgs e)
        {
            this.btnDonate.Image = Properties.Resources.Donate_Button_Hover;
        }

        private void btnDonate_MouseLeave(object sender, EventArgs e)
        {
            this.btnDonate.Image = Properties.Resources.Donate_Button_Normal;
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            ToolTip myToolTip = null;
            try
            {
                using (myToolTip = new ToolTip())
                {
                    myToolTip.SetToolTip(this.btnDonate, Resources.AboutBox_DonateBtn_ToolTip);
                    myToolTip.SetToolTip(this.lblEmail, Resources.AboutBox_AboutBox_Load_Mailto);
                    myToolTip.SetToolTip(this.logoPictureBox, Resources.AboutBox_PicBox_ToolTip);
                }
            }
            catch
            {
            }
        }

        private void copyEmailToolStripMenuItem_Click(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                {
                    ctxmsEmail.Show();
                    break;
                }

                case MouseButtons.Left:
                {
                    Process.Start("mailto:" + this.lblEmail.Text);
                    break;
                }
            }
        }

        private void copyEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.lblEmail.Text);
        }

        private void logoPictureBox_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://zhivkosk.wordpress.com/");
        }
    }
}
