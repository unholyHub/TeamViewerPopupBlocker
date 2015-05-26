using System;
using System.Threading;
using System.Windows.Forms;

namespace TeamViewerPopupBlocker
{
    public static class Program
    {
        private static Mutex mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            bool result;
            mutex = new Mutex(false, "5cb4795f-203f-4db7-bd9b-133c6d0565b1", out result);

            if (!result)
            {
                MessageBox.Show("Another instance is already running.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            GC.KeepAlive(mutex);
        }
    }
}
