namespace System.Windows.Forms {
    using System.Threading;
    using System.Windows.Forms.Extensions;

    public class AutoClosingMessageBox {
        readonly string caption;
        readonly DialogResult result;
        AutoClosingMessageBox(string text, string caption, int timeout, 
            MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None) {
            this.caption = caption;
            this.result = buttons.ToDialogResult(defaultResult);
            using(new System.Threading.Timer(OnTimerElapsed, result.ToDialogButtonId(buttons), timeout, Timeout.Infinite))
                this.result = MessageBox.Show(text, caption, buttons);
        }
        void OnTimerElapsed(object state) {
            CloseMessageBoxWindow((int)state);
        }
        void CloseMessageBoxWindow(int dlgButtonId) {
            IntPtr hWndMsgBox = Utils.Win32Api.FindMessageBox(caption);
            Utils.Win32Api.SendCommandToDlgButton(hWndMsgBox, dlgButtonId);
        }
        #region API
        public static DialogResult Show(string text,
            string caption = null, int timeout = 1000,
            MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None) {
            return new AutoClosingMessageBox(text, caption ?? string.Empty, timeout, buttons, defaultResult).result;
        }
        #endregion
    }
}