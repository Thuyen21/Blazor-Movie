using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class AdminAccountManagement
    {
        [Parameter]
        public List<AccountManagementModel> accs { get; set; } = new();
        protected int index = 0;
        protected string? searchString { get; set; }
        protected bool isSearch = false;

        protected string sort = null;

        protected async Task NameSortParm()
        {
            index = 0;
            sort = sort == "name" ? "nameDesc" : "name";
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
            isSearch = false;
            searchString = null;




        }

        protected async Task DateSortParm()
        {
            index = 0;
            sort = sort == "date" ? "dateDesc" : "date";
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
            isSearch = false;
            searchString = null;



        }

        protected async Task EmailSortParm()
        {
            index = 0;
            sort = sort == "email" ? "emailDesc" : "email";
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
            isSearch = false;
            searchString = null;



        }
        protected async Task Search()
        {
            index = 0;
            if (searchString != null)
            {
                accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/{searchString}/ /{index}");
                isSearch = true;
            }
            else
            {
                accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}");
                isSearch = false;
            }


            sort = null;
        }
        protected override async Task OnInitializedAsync()
        {
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}");

        }
        protected async Task LoadMore()
        {
            index++;
            if (isSearch)
            {
                accs.AddRange(await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/{searchString}//{index}"));
            }
            else if (sort != null)
            {
                accs.AddRange(await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ /{sort}/{index}"));
            }
            else
            {
                accs.AddRange(await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}"));
            }

        }
    }
}