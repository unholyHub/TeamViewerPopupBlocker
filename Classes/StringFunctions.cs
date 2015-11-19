using System;
using System.Globalization;

namespace TeamViewerPopupBlocker.Classes
{
    public static class StringFunctions
    {
        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="inputString">
        /// The source string.
        /// </param>
        /// <param name="startIndex">
        /// The source start index.
        /// </param>
        /// <param name="length">
        /// The source length.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> equivalent to the substring of length that begins at startIndex in this instance, or <see cref="string.Empty"/> if startIndex is equal to the length of this instance and length is zero.
        /// </returns>
        public static string SubstrString(string inputString, int startIndex, int length)
        {
            string substring = null;
            try
            {
                substring = inputString.Substring(startIndex, length);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogSystem.Instance.AddToLog(ex);
            }

            return substring;
        }

        /// <exception cref="FormatException">The length of <paramref name="format" /> is 1, and it is not one of the format specifier characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo" />.-or- <paramref name="format" /> does not contain a valid custom format pattern. </exception>
        /// <exception cref="ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by <paramref name="provider" />. </exception>
        public static string GetCurrentTimeString
        {
            get
            {
                DateTime dateTime = DateTime.Now;

                string currentTime = null;
                try
                {
                    currentTime =
                        dateTime.ToString(
                            $"{dateTime.Day:00}.{dateTime.Month:00}.{dateTime.Year:0000} - {dateTime.Hour:00}:{dateTime.Minute:00}:{dateTime.Second:00}:{dateTime.Millisecond:000}"
                            , CultureInfo.InvariantCulture);
                }
                catch (ArgumentOutOfRangeException argument)
                {
                    LogSystem.Instance.AddToLog(argument);
                }
                catch (FormatException formatException)
                {
                    LogSystem.Instance.AddToLog(formatException);
                }

                return currentTime;
            }
        }
    }
}