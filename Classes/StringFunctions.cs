//-----------------------------------------------------------------------
// <copyright file="StringFunctions.cs" company="Zhivko">
//     Copyright (c) Zhivko. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Classes
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Class for string operations
    /// </summary>
    public static class StringFunctions
    {
        /// <summary>
        ///     Gets the value of the current System.DateTime object to its equivalent string
        ///     representation using the specified format and culture-specific format information.
        /// </summary>
        /// <returns>Current in this format - dd.MM.yyyy - HH:mm:ss:fff</returns>
        public static string GetCurrentTimeString
        {
            get
            {
                DateTime dateTime = DateTime.Now;

                string currentTime = null;

                try
                {
                    currentTime = dateTime.ToString("dd.MM.yyyy - HH:mm:ss:fff", CultureInfo.InvariantCulture);
                }
                catch (ArgumentOutOfRangeException argument)
                {
                    LogSystem.Instance.AddToLog(argument, false);
                }
                catch (FormatException formatException)
                {
                    LogSystem.Instance.AddToLog(formatException, false);
                }

                return currentTime;
            }
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="inputText">The source string. </param>
        /// <param name="startIndex">The source start index. </param>
        /// <param name="length"> The source length. </param>
        /// <returns>
        /// A <see cref="string"/> equivalent to the substring of length that begins at startIndex in this instance, or <see cref="string.Empty"/> if startIndex is equal to the length of this instance and length is zero.
        /// </returns>
        public static string SubstringString(string inputText, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return string.Empty;
            }

            string outputText = string.Empty;

            try
            {
                outputText = inputText.Substring(startIndex, length);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogSystem.Instance.AddToLog(ex, true);
            }

            return outputText;
        }        
    }
}