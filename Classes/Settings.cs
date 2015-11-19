using System;
using System.Collections.ObjectModel;
using System.IO;
using TeamViewerPopupBlocker.Properties;

namespace TeamViewerPopupBlocker.Classes
{
    public class Settings
    {
        public Collection<string> WindowNames { get; set; } = new Collection<string>()
        {
            Resources.TeamViewer_WindowName_Commercial_use,
            Resources.TeamViewer_WindowName_Sponssored_Session,
            Resources.TeamViewer_WindowName_Commercial_use_suspected,
            Resources.TeamViewer_WindowName_Unable_to_connect
        };
        
        public string PayPalUrl { get; private set; } = @"https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=SA8FZ39F68AKW";

        /// <summary>
        /// The sync root to ensure that only one instance is running.
        /// </summary>
        private static readonly object SyncRoot = new object();

        private static Settings _instance;

        public string ProgramName { get; } = "TeamViewer Popup Blocker";

        public string ProgramAppDataDirectory { get; set; }

        private string WindowNamesTxt { get; set; } = @"tvpb Window Names.txt";

        public static Settings Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    return _instance ?? (_instance = new Settings());
                }
            }
        }

        private Settings()
        {
            Init();
        }

        private void Init()
        {
            ProgramAppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.ProgramName);
            CheckForAppDir();
            WindowNamesTxt = Path.Combine(ProgramAppDataDirectory, WindowNamesTxt);

            if (!File.Exists(WindowNamesTxt))
            {
                FileOperations.AddTextToFile(WindowNamesTxt, WindowNames);
            }
        }

        private void CheckForAppDir()
        {
            if (!Directory.Exists(ProgramAppDataDirectory))
            {
                Directory.CreateDirectory(ProgramAppDataDirectory);
            }
        }

        public void Save()
        {
            FileOperations.WriteWindowNamesToFile(WindowNamesTxt, WindowNames);
        }


        public void Load()
        {
            WindowNames = FileOperations.ReadTextFromFile(WindowNamesTxt);
        }
    }
}
