using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

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
