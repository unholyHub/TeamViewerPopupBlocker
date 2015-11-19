using System;
using System.Threading;
using System.Windows.Forms;
using TeamViewerPopupBlocker.Forms;
using TeamViewerPopupBlocker.Properties;
using Settings = TeamViewerPopupBlocker.Classes.Settings;

namespace TeamViewerPopupBlocker
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">The named mutex exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl" />.</exception>
        /// <exception cref="System.IO.IOException">A Win32 error occurred.</exception>
        /// <exception cref="WaitHandleCannotBeOpenedException">The named mutex cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
        [STAThread]
        public static void Main()
        {
            bool result;
            using (Mutex mutex = new Mutex(false, "5cb4795f-203f-4db7-bd9b-133c6d0565b1", out result))
            {
                if (!result)
                {
                    MessageBox.Show(Resources.Program_MsgBox,
                        Settings.Instance.ProgramName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1, 0);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
