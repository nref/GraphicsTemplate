using System;
using System.Runtime.InteropServices;

namespace GraphicsTemplate.Shared
{
    public class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
    }
}
