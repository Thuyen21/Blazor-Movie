using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class EditAccount
    {
        [Parameter]
        public string Id { get; set; }

        protected AccountManagementModel acc;
        protected string content;
        protected override async Task OnInitializedAsync()
        {
            if (Id == null)
            {
                _navigationManager.NavigateTo("/AdminAccountManagement");
            }

            acc = await _httpClient.GetFromJsonAsync<AccountManagementModel>($"Admin/EditAccount/{Id}");
        }

        protected async Task HandleValidSubmit()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<AccountManagementModel>("Admin/EditAccount", acc);
            content = await response.Content.ReadAsStringAsync();
        }
    }
}