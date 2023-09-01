using System;
using System.Runtime.InteropServices;

namespace Calculator
{
    class W32
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public extern static int GetWindowLong(IntPtr hWnd, GWL nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public extern static int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(int hWnd, int wMsg, int wParam, Int32 lParam);
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public extern static bool ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "ShowWindow", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern Int32 ShowWindow(Int32 hwnd, Int32 nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
        [DllImport("user32", EntryPoint = "RemoveMenu", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern Int32 RemoveMenu(Int32 hMenu, Int32 nPosition, Int32 wFlags);
        [DllImport("user32", EntryPoint = "AppendMenuA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern Int32 AppendMenu(Int32 hMenu, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);
        [DllImport("user32")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint itemId, uint uEnable);
        [DllImport("user32.dll ")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
            public MARGINS(int Left, int Right, int Top, int Bottom) : this()
            {
                cxLeftWidth = Left;
                cxRightWidth = Right;
                cyTopHeight = Top;
                cyBottomHeight = Bottom;
            }
        }

        public const uint TPM_LEFTBUTTON = 0x0000;
        public const uint TPM_RETURNCMD = 0x0100;
        public const uint WM_SYSCOMMAND = 0x0112;

        public static int WS_OVERLAPPED = 0;
        public static UInt32 WS_POPUP = 0x80000000;
        public static int WS_CHILD = 0x40000000;
        public static int WS_MINIMIZE = 0x20000000;
        public static int WS_VISIBLE = 0x10000000;
        public static int WS_DISABLED = 0x8000000;
        public static int WS_CLIPSIBLINGS = 0x4000000;
        public static int WS_CLIPCHILDREN = 0x2000000;
        public static int WS_MAXIMIZE = 0x1000000;
        public static int WS_CAPTION = 0xC00000;      // WS_BORDER or WS_DLGFRAME  
        public static int WS_BORDER = 0x800000;
        public static int WS_DLGFRAME = 0x400000;
        public static int WS_VSCROLL = 0x200000;
        public static int WS_HSCROLL = 0x100000;
        public static int WS_SYSMENU = 0x80000;
        public static int WS_THICKFRAME = 0x40000;
        public static int WS_GROUP = 0x20000;
        public static int WS_TABSTOP = 0x10000;
        public static int WS_MINIMIZEBOX = 0x20000;
        public static int WS_MAXIMIZEBOX = 0x10000;
        public static int WS_TILED = WS_OVERLAPPED;
        public static int WS_ICONIC = WS_MINIMIZE;
        public static int WS_SIZEBOX = WS_THICKFRAME;

        public static int WS_EX_DLGMODALFRAME = 0x0001;
        public static int WS_EX_NOPARENTNOTIFY = 0x0004;
        public static int WS_EX_TOPMOST = 0x0008;
        public static int WS_EX_ACCEPTFILES = 0x0010;
        public static int WS_EX_TRANSPARENT = 0x0020;
        public static int WS_EX_MDICHILD = 0x0040;
        public static int WS_EX_TOOLWINDOW = 0x0080;
        public static int WS_EX_WINDOWEDGE = 0x0100;
        public static int WS_EX_CLIENTEDGE = 0x0200;
        public static int WS_EX_CONTEXTHELP = 0x0400;
        public static int WS_EX_RIGHT = 0x1000;
        public static int WS_EX_LEFT = 0x0000;
        public static int WS_EX_RTLREADING = 0x2000;
        public static int WS_EX_LTRREADING = 0x0000;
        public static int WS_EX_LEFTSCROLLBAR = 0x4000;
        public static int WS_EX_RIGHTSCROLLBAR = 0x0000;
        public static int WS_EX_CONTROLPARENT = 0x10000;
        public static int WS_EX_STATICEDGE = 0x20000;
        public static int WS_EX_APPWINDOW = 0x40000;
        public static int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public static int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        public static int WS_EX_LAYERED = 0x00080000;
        public static int WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        public static int WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
        public static int WS_EX_COMPOSITED = 0x02000000;
        public static int WS_EX_NOACTIVATE = 0x08000000;

        public static int WM_NCLBUTTONDOWN = 0xA1;
        public static int HTBOTTOM = 15;
        public static int HTBOTTOMLEFT = 16;
        public static int HTBOTTOMRIGHT = 17;
        public static int HTCAPTION = 2;
        public static int HTLEFT = 10;
        public static int HTRIGHT = 11;
        public static int HTTOP = 12;
        public static int HTTOPLEFT = 13;
        public static int HTTOPRIGHT = 14;


        public enum GWL : int
        {
            ExStyle = -20,
            Style = -16
        }
    }
}
