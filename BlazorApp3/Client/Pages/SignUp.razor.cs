using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class SignUp
    {
        protected SignUpModel signUpModel = new()
        { DateOfBirth = DateTime.Now };
        protected string content;
        protected async Task HandleValidSubmit()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<SignUpModel>("user/signUp", signUpModel);
            if (response.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("/", true);
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }
        }
    }
}