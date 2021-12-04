using MudBlazor;

namespace MovieClient.Services;

public class ShowAlertService
{
    public string? content;
    public bool showAlert;
    public Severity severity;

    public ShowAlertService()
    {
        content = null;
        showAlert = false;
        severity = new Severity();
    }
    public void CloseAlert()
    {
        showAlert = false;
    }
    public void ShowAlert(Severity severity, string content)
    {
        this.severity = severity;
        this.content = content;
        showAlert = true;
    }
    public void ShowAlert(bool? severity, string content)
    {
        if (severity == null)
        {
            this.severity = Severity.Warning;
        }
        else if (severity == true)
        {
            this.severity = Severity.Success;
        }
        else
        {
            this.severity = Severity.Error;
        }
        this.content = content;
        showAlert = true;
    }
}

