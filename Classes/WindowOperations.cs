//-----------------------------------------------------------------------
// <copyright file="WindowOperations.cs" company="Zhivko Kabaivanov">
//     Copyright (c) Zhivko Kabaivanov. All rights reserved.
// </copyright>
// <author>Zhivko Kabaivanov</author>
//-----------------------------------------------------------------------
namespace TeamViewerPopupBlocker.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;   

    /// <summary>
    /// Class for window operations.
    /// </summary>
    public static class WindowOperations
    {      
        /// <summary>
        /// Sent as a signal that a window or an application should terminate. 
        /// A window receives this message through its WindowProc function.
        /// </summary>
        private const int WM_CLOSE = 0x0010;

        /// <summary>
        /// Sent to a window to retrieve a handle to the large or small icon associated with a window. 
        /// The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption.
        /// A window receives this message through its WindowProc function.
        /// </summary>
        private const int WM_GETICON = 0x007F;

        /// <summary>
        /// Retrieves a handle to the icon associated with the class.
        /// </summary>
        private const int GCL_HICON = -14;

        /// <summary>
        /// Sent when the user clicks a button.
        /// </summary>
        private const int BN_CLICKED = 245;

        /// <summary>
        /// Retrieves the small icon provided by the application. 
        /// If the application does not provide one, the system uses the system-generated icon for that window.
        /// </summary>
        private static IntPtr ICON_SMALL2 = new IntPtr(2);

        /// <summary>
        /// Default application icon.
        /// </summary>
        private static IntPtr IDI_APPLICATION = new IntPtr(0x7F00);

        /// <summary>
        /// Gets the available opened windows on the Desktop.
        /// </summary>
        private static List<Window> AvailableOpenedWindows { get; } = new List<Window>();

        /// <summary>
        /// Gets or sets a value indicating whether that will be obtaining the icon from a opened window.
        /// </summary>
        private static bool IsGettingIcon { get; set; }     

        /// <summary>
        /// Initializes the <see cref="AvailableOpenedWindows"/> collection with the 
        /// currently opened applications windows on desktop.
        /// </summary>
        /// <param name="isGettingIcon"> Determine whether it will obtain the opened applications windows icons. </param>
        /// <returns> Returns <see cref="IEnumerable{T}"/> of the applications windows. </returns>
        public static IEnumerable<Window> InitOpenWindows(bool isGettingIcon)
        {
            IsGettingIcon = isGettingIcon;
            GetOpenedWindows();
            AvailableOpenedWindows.Sort();

            return AvailableOpenedWindows;
        }

        /// <summary>
        /// Performing a check for the main window title of TeamViewer. If it is in the <see cref="Settings.Instance.WindowNames"/>
        /// </summary>
        public static void CheckTeamViewerMainWindowTitle()
        {
            Process[] processes = Process.GetProcessesByName("TeamViewer");

            if (processes.Length == 0)
            {
                return;
            }

            string currentMainWindowTitle = processes[0].MainWindowTitle;

            if (string.IsNullOrEmpty(currentMainWindowTitle))
            {
                return;
            }

            foreach (string windowName in Settings.Instance.WindowNames.Where(windowName => currentMainWindowTitle == windowName))
            {
                CloseWindowByName(windowName);
            }
        }

        /// <summary>
        /// Closes particular window by a given name.
        /// </summary>
        /// <param name="windowName"> The window name that will be closed. </param>
        private static void CloseWindowByName(string windowName)
        {
            if (string.IsNullOrEmpty(windowName))
            {
                return;
            }

            IntPtr windowPtr = NativeMethods.FindWindowByCaption(IntPtr.Zero, windowName);

            if (windowPtr == IntPtr.Zero)
            {
                return;
            }

            IntPtr sendMsgPtr =  NativeMethods.SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            if (sendMsgPtr == (IntPtr) 1)
            {
                return;
            }

            IntPtr butn = NativeMethods.FindWindowEx(windowPtr, IntPtr.Zero, "Button", "OK");

            if (butn != IntPtr.Zero)
            {
                NativeMethods.SendMessage(butn, BN_CLICKED, IntPtr.Zero, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Retrieves the specified value from the WNDCLASSEX structure associated with the specified window.
        /// 64 bit version maybe loses significant 64-bit specific information
        /// </summary>
        /// <param name="hWnd"> A handle to the window and, indirectly, the class to which the window belongs. </param>
        /// <param name="nIndex"> The value to be retrieved. 
        /// To retrieve a value from the extra class memory, specify the positive, zero-based byte offset of the value to be retrieved. 
        /// Valid values are in the range zero through the number of bytes of extra class memory, minus eight; 
        /// for example, if you specified 24 or more bytes of extra class memory, 
        /// a value of 16 would be an index to the third integer. 
        /// To retrieve any other value from the WNDCLASSEX structure, specify one of the following values.</param>
        /// <returns>
        ///  If the function succeeds, the return value is the requested value.
        ///  If the function fails, the return value is zero. 
        /// </returns>
        private static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return new IntPtr(NativeMethods.GetClassLong32(hWnd, nIndex));
            }

            return NativeMethods.GetClassLong64(hWnd, nIndex);
        }

        /// <summary>
        /// Retries the icon of a given window handle.
        /// </summary>
        /// <param name="hWnd"> The window handle that the icon will be taken. </param>
        /// <returns> Returns <see cref="Image"/> that represents the application icon of a given window. </returns>
        private static Image GetSmallWindowIcon(IntPtr hWnd)
        {
            try
            {
                IntPtr hIcon = default(IntPtr);

                hIcon = NativeMethods.SendMessage(hWnd, WM_GETICON, ICON_SMALL2, IntPtr.Zero);

                if (hIcon == IntPtr.Zero)
                {
                    hIcon = GetClassLongPtr(hWnd, GCL_HICON);
                }

                if (hIcon == IntPtr.Zero)
                {
                    hIcon = NativeMethods.LoadIcon(IntPtr.Zero, IDI_APPLICATION);
                }

                if (hIcon != IntPtr.Zero)
                {
                    return new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 16, 16);
                }

                return null;
            }
            catch (ArgumentNullException argumentNullException)
            {
                LogSystem.Instance.AddToLog(argumentNullException, false);
                return null;
            }
        }

        /// <summary>
        /// Refreshing the <see cref="AvailableOpenedWindows"/> collection with opened application windows.
        /// </summary>
        private static void GetOpenedWindows()
        {
            AvailableOpenedWindows.Clear();

            NativeMethods.EnumWindowsProc callback = new NativeMethods.EnumWindowsProc(Report);
            NativeMethods.EnumWindows(callback, (IntPtr)0);
        }

        /// <summary>
        /// An application-defined callback function used with the EnumWindows or EnumDesktopWindows function. 
        /// It receives top-level window handles. 
        /// The WNDENUMPROC type defines a pointer to this callback function. 
        /// EnumWindowsProc is a placeholder for the application-defined function name.
        /// </summary>
        /// <param name="hwnd"> A handle to a top-level window. </param>
        /// <param name="lParam"> The application-defined value given in EnumWindows or EnumDesktopWindows. </param>
        /// <returns> To continue enumeration, the callback function must return <see cref="true"/>; to stop enumeration, it must return <see cref="false"/>. </returns>
        private static bool Report(IntPtr hwnd, int lParam)
        {
            if (NativeMethods.IsWindowVisible(hwnd) == false)
            {
                return true;
            }

            StringBuilder stringBuilder = new StringBuilder(256);
            int result = NativeMethods.GetWindowText(hwnd, stringBuilder, stringBuilder.Capacity);

            if (result == 0)
            {
                return true;
            }

            string currentWindowName = stringBuilder.ToString();

            if (string.IsNullOrEmpty(currentWindowName))
            {
                return true;
            }

            int found = WindowOperations.AvailableOpenedWindows.FindIndex(item => item.WindowName == currentWindowName);
            if (found != -1)
            {
                return true;
            }

            if (currentWindowName == "Program Manager"
                || currentWindowName == "Add TeamViewer Window Name"
                || currentWindowName == "Available Windows")
            {
                return true;
            }

            Bitmap icon = null;

            if (WindowOperations.IsGettingIcon)
            {
                icon  = WindowOperations.GetSmallWindowIcon(hwnd) as Bitmap;
            }

            WindowOperations.AvailableOpenedWindows.Add(new Window(currentWindowName, icon));

            return true;
        }
    }
}