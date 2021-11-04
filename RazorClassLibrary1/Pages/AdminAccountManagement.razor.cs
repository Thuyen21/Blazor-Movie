using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.JSInterop;
using RazorClassLibrary1;
using RazorClassLibrary1.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RazorClassLibrary1.Services;
using System.Reflection;
using BlazorApp3.Shared;
using MudBlazor;

namespace RazorClassLibrary1.Pages
{
    public partial class AdminAccountManagement
    {
        private List<AccountManagementModel> accs { get; set; } = new();
        private int index = 0;
        private string? searchString { get; set; }

        private bool isSearch = false;
        private string sort = null;
        private async Task NameSortParm()
        {
            index = 0;
            sort = sort == "name" ? "nameDesc" : "name";
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        private async Task DateSortParm()
        {
            index = 0;
            sort = sort == "date" ? "dateDesc" : "date";
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        private async Task EmailSortParm()
        {
            index = 0;
            sort = sort == "email" ? "emailDesc" : "email";
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        private async Task Search()
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

        private async Task LoadMore()
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