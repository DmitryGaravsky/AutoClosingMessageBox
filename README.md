# AutoClosingMessageBox
The MessageBox which automatically closes after the specific timeout.

<a href="https://www.nuget.org/packages/AutoClosingMessageBox/"><img alt="Nuget Version" src="https://img.shields.io/nuget/v/AutoClosingMessageBox.svg" data-canonical-src="https://img.shields.io/nuget/v/AutoClosingMessageBox.svg" style="max-width:100%;" /></a>
<a href="https://stackoverflow.com/a/14522952/1010363"><img alt="StackOverflow Answer" src="https://img.shields.io/badge/StackOverflow-QnA-green.svg"></a>


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


You can also use the `AutoClosingMessageBoxFactory` method to get full control on MessageBox showing:

    var toBeOrNotToBeQuestion = AutoClosingMessageBox.Factory(
            showMethod: (caption, buttons) =>
                MessageBox.Show(this, "To be or not to be?", caption, buttons, MessageBoxIcon.Question),
            caption: "The question"
        );
    if(DialogResult.Yes == toBeOrNotToBeQuestion.Show(
                                timeout: 2500,
                                buttons: MessageBoxButtons.YesNo,
                                defaultResult: DialogResult.Yes)) {
        // to be
    }
    else {
        // or not
    }


### NuGet:
To install [AutoClosingMessageBox](https://www.nuget.org/packages/AutoClosingMessageBox/1.0.0.2), run the following command in the Package Manager Console:

    Install-Package AutoClosingMessageBox -Version 1.0.0.2
