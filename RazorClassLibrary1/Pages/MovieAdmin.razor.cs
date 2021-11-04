using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class MovieAdmin
{
    private List<MovieModel> movies = new();
    private int index = 0;
    private string? searchString { get; set; }

    private bool isSearch = false;
    private string sort = null;
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = null;
    }

    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = null;
    }

    private async Task GenreSortParm()
    {
        index = 0;
        sort = sort == "genre" ? "genreDesc" : "genre";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = null;
    }

    protected override async Task OnInitializedAsync()
    {
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ / /{index}");
    }

    private async Task Search()
    {
        index = 0;
        if (searchString != null)
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/{searchString}/ /{index}");
            isSearch = true;
        }
        else
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ / /{index}");
            isSearch = false;
        }

        sort = null;
    }

    private async Task LoadMore()
    {
        index++;
        if (isSearch)
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/{searchString}//{index}"));
        }
        else if (sort != null)
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ /{sort}/{index}"));
        }
        else
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ / /{index}"));
        }
    }
}
