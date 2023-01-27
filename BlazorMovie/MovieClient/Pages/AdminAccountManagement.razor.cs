using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class AdminAccountManagement
{
    /* A property that is used to store the data that is returned from the API. */
    private List<AccountManagementModel> accs { get; set; } = new();
    /* Used to keep track of the page number. */
    private int index = 0;
    /* A property that is used to store the search string. */
    private string searchString = string.Empty;
    /* Used to keep track of whether the user is searching or not. */
    private bool isSearch = false;
    /* A nullable string. */
    private string? sort = null;
    /// <summary>
    /// This function is used to sort the data in the table by name
    /// </summary>
    /// <summary>
    /// It's an async function that calls an API to get a list of accounts, then sorts them by name
    /// </summary>
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        accs = (await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}"))!;
        isSearch = false;
        searchString = string.Empty;
    }

    /// <summary>
    /// It's an async function that gets a list of AccountManagementModel objects from a web api, and
    /// then sets the index to 0, sets the sort to either "date" or "dateDesc", and sets the isSearch to
    /// false and the searchString to an empty string
    /// </summary>
    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
        accs = (await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}"))!;
        isSearch = false;
        searchString = string.Empty;
    }

    /// <summary>
    /// It's an async function that gets a list of AccountManagementModel objects from a web api, and
    /// then sets the sort string to either email or emailDesc, depending on the current value of the
    /// sort string
    /// </summary>
    private async Task EmailSortParm()
    {
        index = 0;
        sort = sort == "email" ? "emailDesc" : "email";
        accs = (await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"Admin/AccountManagement/ /{sort}/{index}"))!;
        isSearch = false;
        searchString = string.Empty;
    }

    /// <summary>
    /// It's a function that gets data from the server and displays it in a table.
    /// </summary>
    private async Task Search()
    {
        index = 0;
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            accs = (await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/{searchString}/ /{index}"))!;
            isSearch = true;
        }
        else
        {
            accs = (await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}"))!;
            isSearch = false;
        }

        sort = null;
    }

    /// <summary>
    /// It gets the data from the API and stores it in the accs variable.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        accs = (await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}"))!;
    }

    /// <summary>
    /// It loads more data from the server and adds it to the list of data that is already loaded
    /// </summary>
    private async Task LoadMore()
    {
        index++;
        if (isSearch)
        {
            accs.AddRange((await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/{searchString}//{index}"))!);
        }
        else if (sort != null)
        {
            accs.AddRange((await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ /{sort}/{index}"))!);
        }
        else
        {
            accs.AddRange((await _httpClient.GetFromJsonAsync<List<AccountManagementModel>>($"admin/AccountManagement/ / /{index}"))!);
        }
    }
}
