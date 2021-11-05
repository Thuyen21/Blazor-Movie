using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SalaryStudio
{
#pragma warning disable CS8618 // Non-nullable field 'Email' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string Email;
#pragma warning restore CS8618 // Non-nullable field 'Email' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'EmailConfirm' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string EmailConfirm;
#pragma warning restore CS8618 // Non-nullable field 'EmailConfirm' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private double Cash;
#pragma warning disable CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string content;
#pragma warning restore CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
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
