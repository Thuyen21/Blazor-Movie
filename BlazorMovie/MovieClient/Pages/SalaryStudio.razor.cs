using MovieClient.Services;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SalaryStudio
{
    private string? Email;
    private string? EmailConfirm;
    private double Cash;
    private ShowAlertService alertService = new();
    private bool isMail(string Email)
    {
        try
        {
            System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(Email);
            return true;
        }
        catch
        {
            alertService.ShowAlert(Severity.Warning, "Not Email");
            return false;
        }

    }
    private async Task Submit()
    {
        if (isMail(Email))
        {
            if (Email != EmailConfirm)
            {
                alertService.ShowAlert(Severity.Warning, "Check your email");
            }
            else
            {
                HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Studio/Salary", new Dictionary<string, string>()
                {{"Email", Email}, {"Cash", Cash.ToString()}});
                alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
