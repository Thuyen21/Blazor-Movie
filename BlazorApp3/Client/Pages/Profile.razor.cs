using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class Profile
    {
        protected AccountManagementModel accountManagementModel = new AccountManagementModel();
        protected string content;
        protected override async Task OnInitializedAsync()
        {
            accountManagementModel = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/Profile");
        }

        protected async Task HandleValidSubmit()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<AccountManagementModel>("user/EditProfile", accountManagementModel);
            content = await response.Content.ReadAsStringAsync();
        }
    }
}