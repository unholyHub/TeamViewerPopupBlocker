using System;
using System.Drawing;

namespace TeamViewerPopupBlocker.Classes
{
    public class Window : IComparable<Window>
    {
        public Window(string windowName, Bitmap windowIcon)
        {
            this.WindowName = windowName;
            this.WindowIcon = windowIcon;
        }

        public string WindowName { get; }

        public Bitmap WindowIcon { get; }

        public int CompareTo(Window other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            return string.Compare(this.WindowName, other.WindowName, StringComparison.OrdinalIgnoreCase);
        }

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

        public override bool Equals(object obj)
        {
            Window other = obj as Window;

            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            char[] c = this.WindowName.ToCharArray();
            return c[0];
        }

        public static bool operator ==(Window left, Window right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Window left, Window right)
        {
            return !(left == right);
        }

        public static bool operator <(Window left, Window right)
        {
            return (Compare(left, right) < 0);
        }

        public static bool operator >(Window left, Window right)
        {
            return (Compare(left, right) > 0);
        }
    }
}