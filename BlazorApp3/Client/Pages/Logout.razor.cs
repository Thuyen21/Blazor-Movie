using BlazorApp3.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorApp3.Client.Pages
{
    public partial class Logout
    {
        [CascadingParameter]
        public Error Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("user/Logout");
            string content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Reason: {response.ReasonPhrase}, Message: {content}");
            }
            _accountService.Logout();
            _navigationManager.NavigateTo("/");
        }
    }
}