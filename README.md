# AutoClosingMessageBox
The MessageBox which automatically closes after the specific timeout.

### Usage:

Use the `AutoClosingMessageBox.Show` method either as follows: 

    // Fire and forget - it about to be closed after default timeout(1000ms)
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

### NuGet:
To install [AutoClosingMessageBox](https://www.nuget.org/packages/AutoClosingMessageBox/1.0.0.1), run the following command in the Package Manager Console:

    Install-Package AutoClosingMessageBox
