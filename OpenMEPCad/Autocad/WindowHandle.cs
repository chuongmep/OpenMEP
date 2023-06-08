using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Autodesk.DesignScript.Runtime;

namespace OpenMEPCad.Autocad
{
	[IsVisibleInDynamoLibrary(false)]
    public static class WindowHandle
    {
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        /// <summary>
        /// Sets the given window's owner to Revit window.
        /// </summary>
        /// <param name="dialog">Target window.</param>
        public static void SetRevitAsWindowOwner(this Window? dialog)
		{
			if (null == dialog) { return; }
			WindowInteropHelper helper = new WindowInteropHelper(dialog);
			helper.Owner = GetActivateWindow();
            dialog.Closing += SetActivateWindow;
        }
        public static void SetActivateWindow(object sender, CancelEventArgs e)
        {
            SetActivateWindow();
        }
		/// <summary>
		/// Set process revert use revit
		/// </summary>
		/// <returns></returns>
		public static bool SetActivateWindow()
        {
            IntPtr ptr = GetActivateWindow();
            if (ptr != IntPtr.Zero)
            {
                return SetForegroundWindow(ptr);
            }
            return false;
        }

        /// <summary>
        /// return active windows is active
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetActivateWindow()
        {
            return Process.GetCurrentProcess().MainWindowHandle;
        }
		/// <summary>
		/// Finds the Revit window handle.
		/// </summary>
		/// <returns>Revit window handle.</returns>
		public static IntPtr FindRevitWindowHandle()
		{
			try
			{
				IntPtr foundRevitHandle = IntPtr.Zero;
				uint currentThreadID = GetCurrentThreadId();

				// Search for the Revit process with current thread ID.
				Process?[] revitProcesses = Process.GetProcessesByName("Revit");
				Process? foundRevitProcess = null;
				foreach (Process? aRevitProcess in revitProcesses)
				{
					foreach (ProcessThread aThread in aRevitProcess.Threads)
					{
						if (aThread.Id == currentThreadID)
						{
							foundRevitProcess = aRevitProcess;
							break;
						}
					}  // For each thread in the process.

					// When we have found our Revit process, then stop searching.
					if (null != foundRevitProcess) { break; }
				}  // For each revit process found

				// Get the window handle of the Revit process found.
				if (null != foundRevitProcess)
				{
					foundRevitHandle = foundRevitProcess.MainWindowHandle;
				}

				return foundRevitHandle;
			}
			catch (Exception)
			{
				return IntPtr.Zero;
			}
		}

        public static void FindAndSetActive()
        {
            var hwnd = FindRevitWindowHandle();
            SetForegroundWindow(hwnd);
        }

        /// <summary>
        /// bring autocad window to foreground
        /// </summary>
        /// <param name="app"></param>
        public static void FocusToCad(dynamic app)
        {
            //app.WindowState = Autodesk.AutoCAD.Interop.Common.AcWindowState.acMax;
            var hwnd = new IntPtr(app.Application.HWND);
            SetForegroundWindow(hwnd);
            
        }
	}
}
