# AutoClosingMessageBox

The MessageBox which automatically closes after specific timeout.


### Usage:

Use the `AutoClosingMessageBox.Show` method either as follows: 

    // Fire and forget(it about to be closed after 1000ms, default timeout)
    AutoClosingMessageBox.Show("Hello, World!");


or follows:

    // Wait for some result or make the default decision
    var result = AutoClosingMessageBox.Show(
                text: "To be or not to be?", 
                caption: "The question",
                timeout: 2500,
                buttons: MessageBoxButtons.YesNo,
                defaultResult: DialogResult.Yes);
    if(result == DialogResult.Yes) {
        // to be
    }
    else { 
        // or not
    }






