using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class AdminAccountManagement
    {
        [Parameter]
        public List<AccountManagementModel> accs { get; set; } = new();
        protected string NameSort = "name";
        protected string DateSort = "date";
        protected string EmailSort = "email";
        protected string sortOrder = "Id";
        protected string searchString { get; set; }

        protected async Task NameSortParm()
        {
            sortOrder = NameSort;
            NameSort = NameSort == "name" ? "nameDesc" : "name";
            await OnInitializedAsync();
        }

        protected async Task DateSortParm()
        {
            sortOrder = DateSort;
            DateSort = DateSort == "date" ? "dateDesc" : "date";
            await OnInitializedAsync();
        }

        protected async Task EmailSortParm()
        {
            sortOrder = EmailSort;
            EmailSort = EmailSort == "email" ? "emailDesc" : "email";
            await OnInitializedAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(searchString))
            {
                if (string.IsNullOrEmpty(sortOrder))
                {
                    accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>("admin/AccountManagement/ /{no}");
                }
                else
                {
                    accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ /{sortOrder}");
                }
            }
            else
            {
                accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/{searchString}/{sortOrder}");
            }
        }
    }
}