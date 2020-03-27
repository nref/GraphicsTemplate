using System;
using System.Runtime.InteropServices;

namespace GraphicsTemplate.Shared
{
    public class User32
    {
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateWindowEx(int dwExStyle,
            string lpszClassName,
            string lpszWindowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInst,
            [MarshalAs(UnmanagedType.Struct)] object pvParam);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        public static extern bool DestroyWindow(IntPtr hwnd);
    }
}
