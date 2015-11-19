using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace TeamViewerPopupBlocker.Classes
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wnd")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h")]
    public delegate bool Callback(IntPtr hWnd, int lParam);
    
    public static class WinApi
    {
        private static uint WM_GETICON = 0x007f;

        private static IntPtr ICON_SMALL2 = new IntPtr(2);

        private static IntPtr IDI_APPLICATION = new IntPtr(0x7F00);

        private static int GCL_HICON = -14;

        private const uint WM_CLOSE = 0x0010;

        private const int BN_CLICKED = 245;

        private static List<Window> AvailableOpenedWindows { get; } = new List<Window>();

        private static bool IsGettingIcon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        private static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowText(IntPtr hWnd, StringBuilder buffer, int buflen);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(Callback callback, IntPtr extraData);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll")]
        private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        public static void CheckForWindowName(string windowName)
        {
            //testing new method active window
            //string ff = GetActiveWindowTitle();

            //if (ff == windowName)
            //{
            //    CloseWindowByName(windowName);
            //}

            //list method
            GetOpenedWindows();
            foreach (Window openedWindow in AvailableOpenedWindows)
            {
                if (openedWindow.WindowName == windowName)
                {
                    CloseWindowByName(windowName);
                }
            }
        }

        public static IEnumerable<Window> InitOpenWindows(bool isGettingIcon)
        {
            IsGettingIcon = isGettingIcon;
            GetOpenedWindows();
            AvailableOpenedWindows.Sort();

            return AvailableOpenedWindows;
        }

        private static void CloseWindowByName(string windowName)
        {
            IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, windowName);

            if (windowPtr == IntPtr.Zero) return;

            SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            var butn = FindWindowEx(windowPtr, IntPtr.Zero, "Button", "OK");

            if (butn != IntPtr.Zero)
            {
                SendMessage(butn, BN_CLICKED, IntPtr.Zero, IntPtr.Zero);
            }
        }

        /// <summary>
        /// 64 bit version maybe loses significant 64-bit specific information
        /// </summary>
        private static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return new IntPtr(GetClassLong32(hWnd, nIndex));
            }

            return GetClassLong64(hWnd, nIndex);
        }


        private static Image GetSmallWindowIcon(IntPtr hWnd)
        {
            try
            {
                IntPtr hIcon = default(IntPtr);

                hIcon = SendMessage(hWnd, WM_GETICON, ICON_SMALL2, IntPtr.Zero);

                if (hIcon == IntPtr.Zero)
                {
                    hIcon = GetClassLongPtr(hWnd, GCL_HICON);
                }

                if (hIcon == IntPtr.Zero)
                {
                    hIcon = LoadIcon(IntPtr.Zero, IDI_APPLICATION);
                }

                if (hIcon != IntPtr.Zero)
                {
                    return new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 16, 16);
                }

                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Instance.AddToLog(ex);
                return null;
            }
        }

        private static void GetOpenedWindows()
        {
            AvailableOpenedWindows.Clear();

            Callback callback = new Callback(Report);
            EnumWindows(callback, (IntPtr)0);
        }


        private static bool Report(IntPtr hwnd, int lParam)
        {
            if (IsWindowVisible(hwnd) == false)
            {
                return true;
            }

            StringBuilder stringBuilder = new StringBuilder(256);
            GetWindowText(hwnd, stringBuilder, stringBuilder.Capacity);
            string currentWindowName = stringBuilder.ToString();

            if (string.IsNullOrEmpty(currentWindowName))
            {
                return true;
            }

            int found = AvailableOpenedWindows.FindIndex(item => item.WindowName == currentWindowName);
            if (found != -1)
            {
                return true;
            }

            //Window window  = new Window(currentWindowName, null);
            //bool contains = AvailableOpenedWindows.Contains(window);

            //if (!contains)
            //{
            //    return true;
            //}

            if (currentWindowName == "Program Manager"
                || currentWindowName == "Add TeamViewer Window Name"
                || currentWindowName == "Avaliable Windows")
            {
                return true;
            }

            Bitmap icon = null;

            if (IsGettingIcon)
            {
                icon  = GetSmallWindowIcon(hwnd) as Bitmap;
            }

            AvailableOpenedWindows.Add(new Window(currentWindowName, icon));

            return true;
        }

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder buffer = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, buffer, nChars))
            {
                return buffer.ToString();
            }
            
            return null;
        }
    }
}