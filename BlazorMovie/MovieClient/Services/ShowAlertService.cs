using MudBlazor;

namespace MovieClient.Services;

public class ShowAlertService
{
    /* A nullable string. */
    public string? content;
    /* A flag that determines whether or not the alert should be shown. */
    public bool showAlert;
    /* Setting the severity of the alert. */
    public Severity severity;

    /* The constructor for the ShowAlertService class. It is setting the default values for the
    content, showAlert, and severity variables. */
    public ShowAlertService()
    {
        content = null;
        showAlert = false;
        severity = new Severity();
    }
    /// <summary>
    /// It sets the showAlert variable to false, which will cause the alert to disappear
    /// </summary>
    public void CloseAlert()
    {
        showAlert = false;
    }
    /// <summary>
    /// It sets the severity and content of the alert, and then sets the showAlert flag to true.
    /// </summary>
    /// <param name="Severity">This is an enum that I created to determine the type of alert that will
    /// be shown.</param>
    /// <param name="content">The content of the alert.</param>
    public void ShowAlert(Severity severity, string content)
    {
        this.severity = severity;
        this.content = content;
        showAlert = true;
    }
    /// <summary>
    /// If the severity is null, then the severity is set to warning. If the severity is true, then the
    /// severity is set to success. If the severity is false, then the severity is set to error
    /// </summary>
    /// <param name="severity">This is the type of alert you want to show. It can be a warning, success,
    /// or error.</param>
    /// <param name="content">The message you want to display</param>
    public void ShowAlert(bool? severity, string content)
    {
        this.severity = severity == null ? Severity.Warning : severity == true ? Severity.Success : Severity.Error;
        this.content = content;
        showAlert = true;
    }
}

