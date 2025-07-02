using Avalonia.Controls;
using System;
using System.Runtime.InteropServices;

namespace Pandora.Utils;

public static partial class TaskbarFlasher
{
	private static IntPtr? _lastFlashingHandle;

	public static void FlashUntilFocused(Window window)
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			return;

		var handle = GetWindowHandle(window);
		if (handle == IntPtr.Zero)
			return;

		_lastFlashingHandle = handle;

		var fw = new FLASHWINFO
		{
			cbSize = (uint)Marshal.SizeOf<FLASHWINFO>(),
			hwnd = handle,
			dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG,
			uCount = uint.MaxValue,
			dwTimeout = 0
		};

		FlashWindowEx(ref fw);

		window.Activated -= OnWindowActivated;
		window.Activated += OnWindowActivated;
	}

	public static void StopFlashing(Window window)
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			return;

		var handle = GetWindowHandle(window);
		if (handle == IntPtr.Zero || _lastFlashingHandle != handle)
			return;

		var fw = new FLASHWINFO
		{
			cbSize = (uint)Marshal.SizeOf<FLASHWINFO>(),
			hwnd = handle,
			dwFlags = FLASHW_STOP,
			uCount = 0,
			dwTimeout = 0
		};

		FlashWindowEx(ref fw);

		_lastFlashingHandle = null;
	}

	private static void OnWindowActivated(object? sender, EventArgs e)
	{
		if (sender is Window window)
		{
			StopFlashing(window);
			window.Activated -= OnWindowActivated;
		}
	}

	private static IntPtr GetWindowHandle(Window window)
	{
		return window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
	}

	#region WinAPI

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

	[StructLayout(LayoutKind.Sequential)]
	private struct FLASHWINFO
	{
		public uint cbSize;
		public IntPtr hwnd;
		public uint dwFlags;
		public uint uCount;
		public uint dwTimeout;
	}

	private const uint FLASHW_STOP = 0;
	private const uint FLASHW_CAPTION = 0x00000001;
	private const uint FLASHW_TRAY = 0x00000002;
	private const uint FLASHW_ALL = FLASHW_CAPTION | FLASHW_TRAY;
	private const uint FLASHW_TIMERNOFG = 0x0000000C;

	#endregion
}
