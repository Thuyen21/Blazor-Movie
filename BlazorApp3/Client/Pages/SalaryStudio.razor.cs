using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class SalaryStudio
    {
        protected string Email;
        protected string EmailConfirm;
        protected double Cash;
        protected string content;
        protected async Task Submit()
        {
            try
            {
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(Email);
            }
            catch
            {
                content = "Not Email";
                return;
            }

            if (Email != EmailConfirm)
            {
                content = "Check your email";
            }
            else
            {
                content = await (await _httpClient.PostAsJsonAsync<Dictionary<string, string>>("Studio/Salary", new Dictionary<string, string>()
                {{"Email", Email}, {"Cash", Cash.ToString()}})).Content.ReadAsStringAsync();
            }
        }
    }
}