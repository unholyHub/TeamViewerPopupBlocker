//-----------------------------------------------------------------------
// <copyright file="Window.cs" company="Zhivko Kabaivanov">
//     Copyright (c) Zhivko Kabaivanov. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Classes
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Class for storing the window name and window icon.
    /// </summary>
    public class Window : IComparable<Window>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="windowName">The window name.</param>
        /// <param name="windowIcon">The window icon.</param>
        public Window(string windowName, Bitmap windowIcon)
        {
            this.WindowName = windowName;
            this.WindowIcon = windowIcon;
        }

        /// <summary>
        /// Gets the window name.
        /// </summary>
        public string WindowName { get; }

        /// <summary>
        /// Gets the window icon.
        /// </summary>
        public Bitmap WindowIcon { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Window"/> instances are the same instance.
        /// </summary>
        /// <param name="left"> The first <see cref="Window"/> to compare. </param>
        /// <param name="right"> The second <see cref="Window"/> to compare. </param>
        /// <returns> <see cref="true"/> if left is the same instance as right or if both are <see cref="null"/>; otherwise, <see cref="false"/>.</returns>
        public static bool operator ==(Window left, Window right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Window"/> instances are not the same instance.
        /// </summary>
        /// <param name="left"> The first <see cref="Window"/> to compare. </param>
        /// <param name="right"> The second <see cref="Window"/> to compare. </param>
        /// <returns> <see cref="false"/> if left and right are the same instance <see cref="null"/>; otherwise, <see cref="true"/>.</returns>
        public static bool operator !=(Window left, Window right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares two <see cref="Window"/> and witch is greater.
        /// </summary>
        /// <param name="left"> The first <see cref="Window"/> to compare. </param>
        /// <param name="right"> The second <see cref="Window"/> to compare. </param>
        /// <returns> <see cref="true"/> if the right is greater, otherwise <see cref="false"/>. </returns>
        public static bool operator <(Window left, Window right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        /// Compares two <see cref="Window"/> and witch is greater.
        /// </summary>
        /// <param name="left"> The first <see cref="Window"/> to compare. </param>
        /// <param name="right"> The second <see cref="Window"/> to compare. </param>
        /// <returns> <see cref="true"/> if the left is greater, otherwise <see cref="false"/>. </returns>
        public static bool operator >(Window left, Window right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        /// Compares two specified <see cref="Window"/> objects and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="left">The left <see cref="Window"/> to compare.</param>
        /// <param name="right">The right <see cref="Window"/> to compare.</param>
        /// <returns>   A 32-bit signed integer that indicates the lexical relationship between the two
        ///             comparands.Value Condition Less than zero left is less than right. Zero left equals
        ///             right. Greater than zero left is greater than right.
        /// </returns>
        public static int Compare(Window left, Window right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }

            if (object.ReferenceEquals(left, null))
            {
                return -1;
            }

            return left.CompareTo(right);
        }

        /// <summary>
        /// Compares the <see cref="Window"/> instance with another <see cref="Window"/> object and returns an integer that indicates whether the current instance precedes,
        /// follows, or occurs in the same position in the sort order as the other <see cref="Window"/>.
        /// </summary>
        /// <param name="other">The other <see cref="Window"/>.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. </returns>
        public int CompareTo(Window other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            return string.Compare(this.WindowName, other.WindowName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the hash code for this <see cref="Window"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            char[] c = this.WindowName.ToCharArray();
            return c[0];
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"> The <see cref="object"/> to compare with the current object.</param>
        /// <returns> 
        ///     <see>
        ///         <cref>true</cref>
        ///     </see> if the specified object is equal to the current object; otherwise, <see>
        ///         <cref>false</cref>
        ///     </see>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Window other = obj as Window;

            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.CompareTo(other) == 0;
        }                  
    }
}