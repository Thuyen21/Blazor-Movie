using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SalaryStudio
{
    /* A nullable string. */
    private string? Email;
    /* A nullable string. */
    private string? EmailConfirm;
    /* A variable that is used to store the amount of money that the user wants to send. */
    private double Cash;

    /// <summary>
    /// It takes a string and returns a boolean
    /// </summary>
    /// <param name="Email">The email address to validate.</param>
    /// <returns>
    /// A boolean value.
    /// </returns>
    private bool isMail(string Email)
    {
        try
        {
            System.Net.Mail.MailAddress addr = new(Email);
            return true;
        }
        catch
        {
            alertService.ShowAlert(Severity.Warning, "Not Email");
            return false;
        }

    }
    /// <summary>
    /// If the email is valid, then if the email and email confirmation are the same, then send a post
    /// request to the server with the email and cash amount. If the response is successful, show an
    /// alert with the response message. If the email and email confirmation are not the same, show an
    /// alert with a warning message. If the email is not valid, show an alert with a warning message.
    /// </summary>
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
