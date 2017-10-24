//-----------------------------------------------------------------------// <copyright file="Startup.cs" company="Zhivko Kabaivanov">//     Copyright (c) Zhivko Kabaivanov. All rights reserved.// </copyright>// <author>Zhivko Kabaivanov</author>//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Forms;
    using Forms;
    using Properties;

    /// <summary>
    /// Class for starting the application for blocking the TeamViewer popups.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            bool result;
            using (Mutex mutex = new Mutex(false, "5cb4795f-203f-4db7-bd9b-133c6d0565b1", out result))
            {
                if (!result)
                {
                    var docReaderProcess = Process.GetProcessesByName(Resources.Program_Name);

                    var mainProcess = Process.GetCurrentProcess();
                    
                    foreach (var currentProcess in docReaderProcess)
                    {
                        if (currentProcess.Id != mainProcess.Id)
                        {
                            currentProcess.Kill();
                        }
                    }

                    StartApplication();
                    return;
                }

                StartApplication();
            }
        }

        /// <summary>
        /// Starting the <see cref="MainForm"/> form.
        /// </summary>
        private static void StartApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
