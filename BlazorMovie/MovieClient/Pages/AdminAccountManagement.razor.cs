using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class AdminAccountManagement
{
    private List<AccountManagementModel> accs { get; set; } = new();
    private int index = 0;
    private string? searchString { get; set; }

    private bool isSearch = false;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    private string sort = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
#pragma warning disable CS8601 // Possible null reference assignment.
        accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
        isSearch = false;
        searchString = null;
    }

    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
#pragma warning disable CS8601 // Possible null reference assignment.
        accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
        isSearch = false;
        searchString = null;
    }

    private async Task EmailSortParm()
    {
        index = 0;
        sort = sort == "email" ? "emailDesc" : "email";
#pragma warning disable CS8601 // Possible null reference assignment.
        accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
        isSearch = false;
        searchString = null;
    }

    private async Task Search()
    {
        index = 0;
        if (searchString != null)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/{searchString}/ /{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
            isSearch = true;
        }
        else
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
            isSearch = false;
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        sort = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    protected override async Task OnInitializedAsync()
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        accs = await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    private async Task LoadMore()
    {
        index++;
        if (isSearch)
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<AccountManagementModel>.AddRange(IEnumerable<AccountManagementModel> collection)'.
            accs.AddRange(await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/{searchString}//{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<AccountManagementModel>.AddRange(IEnumerable<AccountManagementModel> collection)'.
        }
        else if (sort != null)
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<AccountManagementModel>.AddRange(IEnumerable<AccountManagementModel> collection)'.
            accs.AddRange(await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ /{sort}/{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<AccountManagementModel>.AddRange(IEnumerable<AccountManagementModel> collection)'.
        }
        else
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<AccountManagementModel>.AddRange(IEnumerable<AccountManagementModel> collection)'.
            accs.AddRange(await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<AccountManagementModel>.AddRange(IEnumerable<AccountManagementModel> collection)'.
        }
    }
}
