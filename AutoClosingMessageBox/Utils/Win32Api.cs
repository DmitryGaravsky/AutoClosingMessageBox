namespace Utils {
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    [SecuritySafeCritical]
    static class Win32Api {
        public static IntPtr FindMessageBox(string caption) {
            const string lpClassName_MessageBox = "#32770";
            return UnsafeNativeMethods.FindWindow(lpClassName_MessageBox, caption);
        }
        public static void SendCommandToDlgButton(IntPtr hWnd, int dlgButtonId) {
            if(hWnd == IntPtr.Zero)
                return;
            UnsafeNativeMethods.EnumChildWindows(hWnd, (handle, param) => {
                int ctrlId = UnsafeNativeMethods.GetDlgCtrlID(handle);
                if(ctrlId == dlgButtonId) {
                    const uint WM_COMMAND = 0x0111;
                    UnsafeNativeMethods.PostMessage(hWnd, WM_COMMAND, new IntPtr(ctrlId), handle);
                }
                return ctrlId != dlgButtonId;
            }, IntPtr.Zero);
        }
        #region SecurityCritical
        static class UnsafeNativeMethods {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr FindWindow(
                [In, Optional] string lpClassName,
                [In, Optional] string lpWindowName
            );
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EnumChildWindows(
                [In, Optional] IntPtr hWndParent,
                [In] EnumChildProc lpEnumFunc,
                [In] IntPtr lParam
            );
            [return: MarshalAs(UnmanagedType.Bool)]
            internal delegate bool EnumChildProc(
                [In] IntPtr hWnd,
                [In] IntPtr lParam
            );
            [DllImport("user32.dll")]
            internal static extern int GetDlgCtrlID(
                [In] IntPtr hWndCtrl
            );
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PostMessage(
                [In] IntPtr hWnd,
                [In] UInt32 Msg,
                [In] IntPtr wParam,
                [In] IntPtr lParam
            );
        }
        #endregion SecurityCritical
    }
}