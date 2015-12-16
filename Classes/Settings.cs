//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="Zhivko Kabaivanov">
//     Copyright (c) Zhivko Kabaivanov. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Classes
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using Properties;

    /// <summary>
    /// Class containing the application settings and utilities.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The sync root to ensure that only one instance is running.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// Instance of the <see cref="Settings"/> class.
        /// </summary>
        private static Settings instance;

        /// <summary>
        /// Field for storing the application current version.
        /// </summary>
        private string applicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Filed storing the window names.
        /// </summary>
        private Collection<string> windowNames = new Collection<string>()
        {
            Resources.TeamViewer_WindowName_Commercial_use,
            Resources.TeamViewer_WindowName_Sponssored_Session,
            Resources.TeamViewer_WindowName_Commercial_use_suspected,
            Resources.TeamViewer_WindowName_Commercial_use_detected,
            Resources.TeamViewer_WindowName_Unable_to_connect
        };

        /// <summary>
        /// Field storing the PayPayUrl.
        /// </summary>
        private Uri payPalUrl = new Uri(@"https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=SA8FZ39F68AKW");
        
        /// <summary>
        /// Prevents a default instance of the <see cref="Settings"/> class from being created.
        /// </summary>
        private Settings()
        {
            this.InitSettings();
        }

        /// <summary>
        /// Gets the instance of the <see cref="Settings"/> class.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    return instance ?? (instance = new Settings());
                }
            }
        }

        /// <summary>
        /// Gets the collection containing the default window names.
        /// </summary>
        public Collection<string> WindowNames
        {
            get { return this.windowNames; }
        }

        /// <summary>
        /// Gets the PayPal URL for donate button.
        /// </summary>
        public Uri PayPalUrl
        {
            get { return this.payPalUrl; }
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                return this.applicationVersion;
            }
        }

        /// <summary>
        /// Gets the path for application data directory.
        /// </summary>
        public string ProgramAppDataDirectory { get; private set; }

        /// <summary>
        /// Gets or sets the file path where the window names are saved.
        /// </summary>
        private string WindowNamesTxtFilePath { get; set; }

        /// <summary>
        /// Saving the window names from <see cref="WindowNames"/> to text file.
        /// </summary>
        public void Save()
        {
            DataFunctions.WriteCollectionToFile(this.WindowNamesTxtFilePath, this.WindowNames);
        }

        /// <summary>
        /// Loading the window names from text file to <see cref="WindowNames"/>.
        /// </summary>
        public void Load()
        {
            this.windowNames = DataFunctions.ReadTextFromFile(this.WindowNamesTxtFilePath);
        }

        /// <summary>
        /// Saving the passed <see cref="Collection{T}"/> to the field.
        /// </summary>
        /// <param name="namesToSave">The passed window names that will be saved to <see cref="Collection{T}"/>.</param>
        public void SaveWindowNames(Collection<string> namesToSave)
        {
            this.windowNames = namesToSave;
        }

        /// <summary>
        /// Initializes the <see cref="Settings"/> properties.
        /// </summary>
        private void InitSettings()
        {
            this.ProgramAppDataDirectory = Path.Combine(
                                                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                        Resources.Program_Name);
            this.CheckForAppDir();

            this.WindowNamesTxtFilePath = @"tvpb Window Names.txt";
            this.WindowNamesTxtFilePath = Path.Combine(this.ProgramAppDataDirectory, this.WindowNamesTxtFilePath);

            if (!File.Exists(this.WindowNamesTxtFilePath))
            {
                DataFunctions.AddCollectionToFile(this.WindowNamesTxtFilePath, this.WindowNames);
            }
        }

        /// <summary>
        /// Performing a check if the application data directory exists, if no
        /// it s created, else nothing happens.
        /// </summary>
        private void CheckForAppDir()
        {
            if (!Directory.Exists(this.ProgramAppDataDirectory))
            {
                Directory.CreateDirectory(this.ProgramAppDataDirectory);
            }
        }        
    }
}
