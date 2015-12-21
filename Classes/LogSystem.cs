//-----------------------------------------------------------------------
// <copyright file="LogSystem.cs" company="Zhivko">
//     Copyright (c) Zhivko. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Classes
{    
    using System;
    using System.IO;
    using Properties;
    
    /// <summary>
    /// Class for logging the behavior of the application.
    /// </summary>
    public sealed class LogSystem
    {
        /// <summary>
        /// The sync root to ensure that only one instance is running.
        /// </summary>
        private static readonly object SyncRoot = new object();
                
        /// <summary>
        /// Instance of the <see cref="LogSystem"/> class.
        /// </summary>
        private static LogSystem instance;          

        /// <summary>
        /// Storing the maximum log file size.
        /// </summary>
        private readonly int logMaxSize;

        /// <summary>
        /// Storing the log file path.
        /// </summary>
        private readonly string logPath; 
        
        /// <summary>
        /// Prevents a default instance of the <see cref="LogSystem"/> class from being created.        
        /// /// </summary>
        private LogSystem()
        {
            this.logPath = Path.Combine(Settings.Instance.ProgramAppDataDirectory, "log.txt");
            File.CreateText(this.logPath).Close();
            this.logMaxSize = 20 * 1024 * 1024;  ////20MB
        }

        /// <summary>
        /// Gets the instance of the <see cref="LogSystem"/> class.
        /// </summary>
        public static LogSystem Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    return instance ?? (instance = new LogSystem());
                }
            }
        }

        /// <summary>
        /// Add exception to the log.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> that will be added to the log.</param>
        /// <param name="isExceptionShort">Determines if the exception added to the log to be short or long.</param>
        public void AddToLog(Exception ex, bool isExceptionShort)
        {
            if (ex == null)
            {
                this.AddToLog(new ArgumentNullException(Resources.LogSystem_AddToLog_The_exception_that_is_adding_to_log_is_null_), false);
                return;
            }

            string literalException = string.Concat("Exception: ", ex.Message);

            if (isExceptionShort == false)
            {
                string exceptionMessage = ex.ToString();

                if (!string.IsNullOrEmpty(exceptionMessage))
                {
                    literalException += " \r\nStackTrace: " + exceptionMessage;
                }
            }

            this.AddToLog(literalException);
        }

        /// <summary>
        /// Adds a string message to the log.
        /// </summary>
        /// <param name="stringData">The string data that will be added to the log.</param>
        private void AddToLog(string stringData)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(this.logPath);
                long fileSize = fileInfo.Length;

                if (fileSize >= this.logMaxSize)
                {
                    if (DataFunctions.LoadStringFromFile(this.logPath) > 0)
                    {
                        string loadedString = DataFunctions.LoadedString;
                        int halfSize = loadedString.Length / 2;
                        loadedString = StringFunctions.SubstringString(loadedString, halfSize, loadedString.Length - halfSize);

                        DataFunctions.SaveTextDataToFile(this.logPath, loadedString, true);
                    }
                }
            }
            catch (UnauthorizedAccessException unathException)
            {
                LogSystem.Instance.AddToLog(unathException, false);
            }

            string outputString = StringFunctions.GetCurrentTimeString + " - " + stringData + "\r\n";
            DataFunctions.SaveTextDataToFile(this.logPath, outputString, true);
        }        
    }
}
