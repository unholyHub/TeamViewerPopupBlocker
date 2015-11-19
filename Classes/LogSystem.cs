using System;
using System.IO;

namespace TeamViewerPopupBlocker.Classes
{
    public sealed class LogSystem
    {
        private static LogSystem instance;

        private static readonly object syncRoot = new object();

        private readonly int logMaxSize;

        private readonly string logPath;

        public static LogSystem Instance
        {
            get
            {
                lock (syncRoot)
                {
                    return instance ?? (instance = new LogSystem());
                }
            }
        }

        private LogSystem()
        {
            this.logPath = Path.Combine(Settings.Instance.ProgramAppDataDirectory, "log.txt");
            this.logMaxSize = 20 * 1024 * 1024; //20MB
        }

        /// <exception cref="UnauthorizedAccessException">Access is denied. </exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
        /// <exception cref="IOException"><see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot update the state of the file or directory. </exception>
        /// <exception cref="FileNotFoundException">The file does not exist.-or- The Length property is called for a directory. </exception>
        public void AddToLog(string data)
        {
            try
            {
                FileInfo tFileInfo = new FileInfo(this.logPath);
                long tFileSize = tFileInfo.Length;

                if (tFileSize >= logMaxSize)
                {
                    if (DataFunctions.LoadStringFromFile(this.logPath) > 0)
                    {
                        string loadedString = DataFunctions.LoadedString;
                        int tHalfSize = loadedString.Length / 2;
                        loadedString = StringFunctions.SubstrString(loadedString, tHalfSize, loadedString.Length - tHalfSize);

                        DataFunctions.SaveStringToFile(this.logPath, loadedString);
                    }
                }
            }
            catch (UnauthorizedAccessException unathException)
            {
                LogSystem.Instance.AddToLog(unathException);
            }

            string tStr = StringFunctions.GetCurrentTimeString + " - " + data + "\r\n";
            DataFunctions.SaveStringToFile(this.logPath, tStr);
        }

        
        /// <exception cref="UnauthorizedAccessException">Access is denied. </exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        public void AddToLog(Exception ex, bool srcShort = false)
        {
            string literalException = string.Concat("Exception: ", ex.Message);

            if (srcShort != true)
            {
                string exceptionMessage = ex.ToString();

                if (!string.IsNullOrEmpty(exceptionMessage))
                {
                    literalException += " \r\nStackTrace: " + exceptionMessage;
                }
            }

            this.AddToLog(literalException);
        }
    }
}
