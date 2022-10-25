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
    using System.Threading;
    using TIMER = System.Threading.Timer;

    public class AutoClosingMessageBox {
        readonly string caption;
        readonly RESULT result;
        AutoClosingMessageBox(string caption, int timeout, Func<string, BUTTONS, RESULT> showMethod,
            BUTTONS buttons = BUTTONS.OK, RESULT defaultResult = RESULT.None, bool showCountDown = false) {
            this.caption = caption ?? string.Empty;
            this.result = buttons.ToDialogResult(defaultResult);
            if(!showCountDown) {
                var timerStrategy = new DelayedCloseUpStrategy(caption, result.ToDialogButtonId(buttons));
                using(new TIMER(OnTimerElapsed, timerStrategy, timeout, Timeout.Infinite))
                    this.result = showMethod(this.caption, buttons);
            }
            else {
                var timerStrategy = new CountDownStrategy(caption, result.ToDialogButtonId(buttons), timeout);
                using(new TIMER(OnCountDownTimer, timerStrategy, 50, 250))
                    this.result = showMethod(this.caption, buttons);
            }
        }
        static void OnTimerElapsed(object state) {
            ((DelayedCloseUpStrategy)state).Proceed();
        }
        static void OnCountDownTimer(object state) {
            ((CountDownStrategy)state).Proceed();
        }
        #region Strategies
        sealed class DelayedCloseUpStrategy {
            readonly string caption;
            readonly int dlgButtonId;
            public DelayedCloseUpStrategy(string caption, int dlgButtonId) {
                this.caption = caption;
                this.dlgButtonId = dlgButtonId;
            }
            public void Proceed() {
                IntPtr hWndMsgBox = Utils.Win32Api.FindMessageBox(caption);
                Utils.Win32Api.SendCommandToDlgButton(hWndMsgBox, dlgButtonId);
            }
        }
        sealed class CountDownStrategy {
            readonly string caption;
            readonly int dlgButtonId;
            readonly System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            IntPtr hWndMsgBox;
            string dlgButtonInitialText;
            TimeSpan timeout;
            public CountDownStrategy(string caption, int dlgButtonId, int timeout) {
                this.caption = caption;
                this.dlgButtonId = dlgButtonId;
                this.timeout = TimeSpan.FromMilliseconds(timeout);
                stopwatch.Start();
            }
            int lastRemainingSeconds = -1;
            public void Proceed() {
                TimeSpan remaining = timeout - stopwatch.Elapsed;
                if(hWndMsgBox == IntPtr.Zero) {
                    hWndMsgBox = Utils.Win32Api.FindMessageBox(caption);
                    dlgButtonInitialText = Utils.Win32Api.GetDlgButtonText(hWndMsgBox, dlgButtonId);
                }
                if(remaining.TotalMilliseconds < 0) {
                    stopwatch.Stop();
                    Utils.Win32Api.SendCommandToDlgButton(hWndMsgBox, dlgButtonId);
                    return;
                }
                int remainingSeconds = (int)Math.Round(remaining.TotalSeconds);
                if(lastRemainingSeconds != remainingSeconds) {
                    Utils.Win32Api.SetDlgButtonText(hWndMsgBox, dlgButtonId, $"{dlgButtonInitialText}({remainingSeconds})");
                    lastRemainingSeconds = remainingSeconds;
                }
            }
        }
        #endregion Strategies
        #region Show API
        public static RESULT Show(string text,
            string caption = null, int timeout = 1000,
            BUTTONS buttons = BUTTONS.OK, RESULT defaultResult = RESULT.None, bool showCountDown = false) {
            return new AutoClosingMessageBox(caption, timeout,
                            (capt, btns) => MessageBox.Show(text, capt, btns),
                        buttons, defaultResult, showCountDown
                    ).result;
        }
        public static RESULT Show(OWNER owner, string text,
            string caption = null, int timeout = 1000,
            BUTTONS buttons = BUTTONS.OK, RESULT defaultResult = RESULT.None, bool showCountDown = false) {
            return new AutoClosingMessageBox(caption, timeout,
                            (capt, btns) => MessageBox.Show(owner, text, capt, btns),
                        buttons, defaultResult, showCountDown
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
            Func<string, BUTTONS, RESULT> showMethod, string caption = null, bool showCountDown = false) {
            if(showMethod == null)
                throw new ArgumentNullException("showMethod");
            return new Impl(showMethod, caption, showCountDown);
        }
        #endregion
        #region IAutoClosingMessageBox
        sealed class Impl : IAutoClosingMessageBox {
            readonly Func<int, BUTTONS, RESULT, RESULT> getResult;
            internal Impl(
                Func<string, BUTTONS, RESULT> showMethod, string caption, bool showCountDown) {
                this.getResult = (timeout, buttons, defaultResult) =>
                    new AutoClosingMessageBox(caption, timeout, showMethod, buttons, defaultResult, showCountDown).result;
            }
            RESULT IAutoClosingMessageBox.Show(
                int timeout, BUTTONS buttons, RESULT defaultResult) {
                return getResult(timeout, buttons, defaultResult);
            }
        }
        #endregion
    }
}