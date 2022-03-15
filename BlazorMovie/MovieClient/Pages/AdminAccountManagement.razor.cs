using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class AdminAccountManagement
{
    private List<UserModel> accs { get; set; } = new();
    private int index = 0;
    private string searchString = string.Empty;
    private string? sort = null;
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        accs = (await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/Admin/UserManagement?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
        accs = (await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/Admin/UserManagement?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    private async Task EmailSortParm()
    {
        index = 0;
        sort = sort == "email" ? "emailDesc" : "email";
        accs = (await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/Admin/UserManagement?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    private async Task Search()
    {
      index = 0;
      accs = (await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/Admin/UserManagement?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    protected override async Task OnInitializedAsync()
    {
        accs = (await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/Admin/UserManagement?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    private async Task LoadMore()
    {
        ++index;
        accs.AddRange((await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/Admin/UserManagement?searchString={searchString}&orderBy={sort}&index={index}"))!);

    }
}
