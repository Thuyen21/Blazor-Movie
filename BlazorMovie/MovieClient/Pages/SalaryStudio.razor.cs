using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SalaryStudio
{
    private string Email;
    private string EmailConfirm;
    private double Cash;
    private string content;
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task Submit()
    {
        try
        {
            System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(Email);
        }
        catch
        {
            content = "Not Email";
            severity = Severity.Error;
            showAlert = true;
            return;
        }

        if (Email != EmailConfirm)
        {
            content = "Check your email";
            severity = Severity.Error;
            showAlert = true;
        }
        else
        {
            HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Studio/Salary", new Dictionary<string, string>()
                {{"Email", Email}, {"Cash", Cash.ToString()}});
            content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                severity = Severity.Success;
            }
            else
            {
                severity = Severity.Error;
            }

            showAlert = true;
        }
    }
}
