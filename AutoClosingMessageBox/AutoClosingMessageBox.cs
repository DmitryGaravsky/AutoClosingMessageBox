namespace System.Windows.Forms {
    using System.Threading;
    using System.Windows.Forms.Extensions;

    public class AutoClosingMessageBox {
        readonly string caption;
        readonly DialogResult result;
        AutoClosingMessageBox(string caption, int timeout, Func<string, MessageBoxButtons, DialogResult> showMethod,
            MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None) {
            this.caption = caption ?? string.Empty;
            this.result = buttons.ToDialogResult(defaultResult);
            using(new Threading.Timer(OnTimerElapsed, result.ToDialogButtonId(buttons), timeout, Timeout.Infinite))
                this.result = showMethod(this.caption, buttons);
        }
        void OnTimerElapsed(object state) {
            CloseMessageBoxWindow((int)state);
        }
        void CloseMessageBoxWindow(int dlgButtonId) {
            IntPtr hWndMsgBox = Utils.Win32Api.FindMessageBox(caption);
            Utils.Win32Api.SendCommandToDlgButton(hWndMsgBox, dlgButtonId);
        }
        #region Show API
        public static DialogResult Show(string text,
            string caption = null, int timeout = 1000,
            MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None) {
            return new AutoClosingMessageBox(caption, timeout,
                            (capt, btns) => MessageBox.Show(text, capt, btns),
                        buttons, defaultResult
                    ).result;
        }
        public static DialogResult Show(IWin32Window owner, string text,
            string caption = null, int timeout = 1000,
            MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None) {
            return new AutoClosingMessageBox(caption, timeout,
                            (capt, btns) => MessageBox.Show(owner, text, capt, btns),
                        buttons, defaultResult
                    ).result;
        }
        #endregion
        #region Factory
        public interface IAutoClosingMessageBox {
            DialogResult Show(
                    int timeout = 1000,
                    MessageBoxButtons buttons = MessageBoxButtons.OK,
                    DialogResult defaultResult = DialogResult.None
                );
        }
        public static IAutoClosingMessageBox Factory(
            Func<string, MessageBoxButtons, DialogResult> showMethod, string caption = null) {
            if(showMethod == null)
                throw new ArgumentNullException(nameof(showMethod));
            return new Impl(showMethod, caption);
        }
        #endregion
        #region IAutoClosingMessageBox
        sealed class Impl : IAutoClosingMessageBox {
            readonly Func<int, MessageBoxButtons, DialogResult, DialogResult> getResult;
            internal Impl(
                Func<string, MessageBoxButtons, DialogResult> showMethod, string caption) {
                this.getResult = (timeout, buttons, defaultResult) =>
                    new AutoClosingMessageBox(caption, timeout, showMethod, buttons, defaultResult).result;
            }
            DialogResult IAutoClosingMessageBox.Show(
                int timeout, MessageBoxButtons buttons, DialogResult defaultResult) {
                return getResult(timeout, buttons, defaultResult);
            }
        }
        #endregion
    }
}