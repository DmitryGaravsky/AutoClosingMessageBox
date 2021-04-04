#if WPF
namespace System.Windows.Controls.Extensions {
    using BUTTONS = System.Windows.MessageBoxButton;
    using RESULT = System.Windows.MessageBoxResult;
#else
namespace System.Windows.Forms.Extensions {
    using RESULT = System.Windows.Forms.DialogResult;
    using BUTTONS = System.Windows.Forms.MessageBoxButtons;
#endif
    static class MessageBoxButtonsExtension {
        public static RESULT ToDialogResult(this BUTTONS buttons, RESULT defaultResult) {
            switch(buttons) {
                case BUTTONS.OK:
                    return RESULT.OK;
                case BUTTONS.OKCancel:
                    if(defaultResult == RESULT.Cancel)
                        break;
                    return RESULT.OK;
                case BUTTONS.YesNo:
                    if(defaultResult == RESULT.No)
                        break;
                    return RESULT.Yes;
                case BUTTONS.YesNoCancel:
                    if(defaultResult == RESULT.No)
                        break;
                    if(defaultResult == RESULT.Cancel)
                        break;
                    return RESULT.Yes;
#if !WPF
                case BUTTONS.RetryCancel:
                    if(defaultResult == RESULT.Retry)
                        break;
                    return RESULT.Cancel;
                case BUTTONS.AbortRetryIgnore:
                    if(defaultResult == RESULT.Abort)
                        break;
                    if(defaultResult == RESULT.Retry)
                        break;
                    return RESULT.Ignore;
#endif
            }
            return defaultResult;
        }
        public static int ToDialogButtonId(this RESULT result, BUTTONS buttons) {
            if(buttons == BUTTONS.OK)
                return 2; // Exceptional case
            return (int)result;
        }
    }
}