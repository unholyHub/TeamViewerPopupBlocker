using System;
using System.IO;
using System.Text;

namespace TeamViewerPopupBlocker.Classes
{
    public static class DataFunctions
    {
        public static string LoadedString { get; private set; }

        /// <exception cref="UnauthorizedAccessException">Access is denied. </exception>
        /// <exception cref="ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed. </exception>
        /// <exception cref="NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream. </exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission. </exception>
        public static void SaveStringToFile(string path, string data, bool append = true)
        {
            string pathWithEnvironmentVariables = Environment.ExpandEnvironmentVariables(path);

            try
            {
                using (StreamWriter sr = new StreamWriter(pathWithEnvironmentVariables, append, Encoding.Unicode))
                {
                    sr.Write(data);
                }
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                LogSystem.Instance.AddToLog(unauthorizedAccessException);
            }
            catch (IOException ioException)
            {
                LogSystem.Instance.AddToLog(ioException);
            }
        }

        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        /// <exception cref="ArgumentException"><paramref name="path" /> is an empty string (""). </exception>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is null. </exception>
        public static int LoadStringFromFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    LoadedString = sr.ReadToEnd();
                    return 1;
                }
            }
            catch (IOException ioException) 
            {
                LogSystem.Instance.AddToLog(ioException);
                return 0;
            }
        }
    }
}
