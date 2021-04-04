#if WPF
namespace System.Windows.Controls {
    using System.Windows.Controls.Extensions;
    using BUTTONS = System.Windows.MessageBoxButton;
    using OWNER = System.Windows.Window;
    using RESULT = System.Windows.MessageBoxResult;
#else
namespace System.Windows.Forms {
    using System.Windows.Forms.Extensions;
    using BUTTONS = System.Windows.Forms.MessageBoxButtons;
    using OWNER = System.Windows.Forms.IWin32Window;
    using RESULT = System.Windows.Forms.DialogResult;
#endif

    public class AutoClosingMessageBox {
        readonly string caption;
        readonly RESULT result;
        AutoClosingMessageBox(string caption, int timeout, Func<string, BUTTONS, RESULT> showMethod,
            BUTTONS buttons = BUTTONS.OK, RESULT defaultResult = RESULT.None) {
            this.caption = caption ?? string.Empty;
            this.result = buttons.ToDialogResult(defaultResult);
            using(new System.Threading.Timer(OnTimerElapsed, result.ToDialogButtonId(buttons), timeout, System.Threading.Timeout.Infinite))
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
        public static RESULT Show(string text,
            string caption = null, int timeout = 1000,
            BUTTONS buttons = BUTTONS.OK, RESULT defaultResult = RESULT.None) {
            return new AutoClosingMessageBox(caption, timeout,
                            (capt, btns) => MessageBox.Show(text, capt, btns),
                        buttons, defaultResult
                    ).result;
        }
        public static RESULT Show(OWNER owner, string text,
            string caption = null, int timeout = 1000,
            BUTTONS buttons = BUTTONS.OK, RESULT defaultResult = RESULT.None) {
            return new AutoClosingMessageBox(caption, timeout,
                            (capt, btns) => MessageBox.Show(owner, text, capt, btns),
                        buttons, defaultResult
                    ).result;
        }
        #endregion
        #region Factory
        public interface IAutoClosingMessageBox {
            RESULT Show(
                    int timeout = 1000,
                    BUTTONS buttons = BUTTONS.OK,
                    RESULT defaultResult = RESULT.None
                );
        }
        public static IAutoClosingMessageBox Factory(
            Func<string, BUTTONS, RESULT> showMethod, string caption = null) {
            if(showMethod == null)
                throw new ArgumentNullException("showMethod");
            return new Impl(showMethod, caption);
        }
        #endregion
        #region IAutoClosingMessageBox
        sealed class Impl : IAutoClosingMessageBox {
            readonly Func<int, BUTTONS, RESULT, RESULT> getResult;
            internal Impl(
                Func<string, BUTTONS, RESULT> showMethod, string caption) {
                this.getResult = (timeout, buttons, defaultResult) =>
                    new AutoClosingMessageBox(caption, timeout, showMethod, buttons, defaultResult).result;
            }
            RESULT IAutoClosingMessageBox.Show(
                int timeout, BUTTONS buttons, RESULT defaultResult) {
                return getResult(timeout, buttons, defaultResult);
            }
        }
        #endregion
    }
}