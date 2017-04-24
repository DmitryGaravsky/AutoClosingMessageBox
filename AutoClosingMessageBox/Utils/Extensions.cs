namespace System.Windows.Forms.Extensions {
    static class MessageBoxButtonsExtension {
        public static DialogResult ToDialogResult(this MessageBoxButtons buttons, DialogResult defaultResult) {
            switch(buttons) {
                case MessageBoxButtons.OK:
                    return DialogResult.OK;
                case MessageBoxButtons.OKCancel:
                    if(defaultResult == DialogResult.Cancel)
                        break;
                    return DialogResult.OK;
                case MessageBoxButtons.YesNo:
                    if(defaultResult == DialogResult.No)
                        break;
                    return DialogResult.Yes;
                case MessageBoxButtons.YesNoCancel:
                    if(defaultResult == DialogResult.No)
                        break;
                    if(defaultResult == DialogResult.Cancel)
                        break;
                    return DialogResult.Yes;
                case MessageBoxButtons.RetryCancel:
                    if(defaultResult == DialogResult.Retry)
                        break;
                    return DialogResult.Cancel;
                case MessageBoxButtons.AbortRetryIgnore:
                    if(defaultResult == DialogResult.Abort)
                        break;
                    if(defaultResult == DialogResult.Retry)
                        break;
                    return DialogResult.Ignore;
            }
            return defaultResult;
        }
    }
}