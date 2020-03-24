using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GraphicsTemplate.Graphics
{
	public interface IGraphicsWindow : IWin32Window
	{
		event SizeChangedEventHandler SizeChanged;
		event RoutedEventHandler Unloaded;
	}

	public class GraphicsWindow : HwndHost, IGraphicsWindow
	{ 
		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			const int WS_CHILD = 0x40000000;

			var hwnd = User32.CreateWindowEx(0, "LISTBOX", "", WS_CHILD,
				0, 0, 0, 0, hwndParent.Handle, IntPtr.Zero, IntPtr.Zero, 0);

			return new HandleRef(this, hwnd);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			User32.DestroyWindow(Handle);
		}
	}
}
