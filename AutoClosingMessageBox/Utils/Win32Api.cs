namespace Utils {
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

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
        public static string GetDlgButtonText(IntPtr hWnd, int dlgButtonId) {
            if(hWnd == IntPtr.Zero)
                return string.Empty;
            StringBuilder sb = new StringBuilder(128);
            int count = (int)UnsafeNativeMethods.GetDlgItemText(hWnd, dlgButtonId, sb, sb.Capacity);
            return sb.ToString(0, Math.Max(0, Math.Min(sb.Length, count)));
        }
        public static bool SetDlgButtonText(IntPtr hWnd, int dlgButtonId, string text) {
            if(hWnd == IntPtr.Zero)
                return false;
            return UnsafeNativeMethods.SetDlgItemText(hWnd, dlgButtonId, text);
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
                [In] IntPtr hDlg
            );
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern uint GetDlgItemText(
                [In] IntPtr hDlg,
                [In] int nIDDlgItem,
                [Out] StringBuilder lpString,
                [In] int nMaxCount
            );
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetDlgItemText(
                [In] IntPtr hDlg,
                [In] int nIDDlgItem,
                [In] string lpString
            );
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool PostMessage(
                [In] IntPtr hWnd,
                [In] uint Msg,
                [In] IntPtr wParam,
                [In] IntPtr lParam
            );
        }
        #endregion SecurityCritical
    }
}