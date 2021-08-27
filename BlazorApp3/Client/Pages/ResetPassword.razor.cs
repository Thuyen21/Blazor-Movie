using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class ResetPassword
    {
        protected ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
        protected string content;
        protected async Task HandleValidSubmit()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<ResetPasswordModel>("user/resetPassword", resetPasswordModel);
            content = await response.Content.ReadAsStringAsync();
        }
    }
}