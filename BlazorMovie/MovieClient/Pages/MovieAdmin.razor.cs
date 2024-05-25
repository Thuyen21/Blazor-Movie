using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class MovieAdmin
{
    /* Initializing the movies variable to an empty list. */
    private List<MovieModel?>? movies = new();
    /* Used to keep track of the page number. */
    private int index = 0;
    /* Initializing the searchString variable to an empty string. */
    private string searchString = string.Empty;
    /* Used to keep track of whether the user is searching or not. */
    private bool isSearch = false;
    /* A nullable string. */
    private string? sort = null;
    /// <summary>
    /// This function is used to sort the movies by name
    /// </summary>
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = string.Empty;
    }

    /// <summary>
    /// It's an async function that gets a list of movies from the server, and then sets the sort
    /// parameter to either "date" or "dateDesc" depending on the current sort parameter
    /// </summary>
    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = string.Empty;
    }

    /// <summary>
    /// It's an async function that gets a list of movies from the server, and then sets the sort
    /// parameter to genre, and the index to 0
    /// </summary>
    private async Task GenreSortParm()
    {
        index = 0;
        sort = sort == "genre" ? "genreDesc" : "genre";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = string.Empty;
    }

    /// <summary>
    /// It gets the movies from the database and displays them on the page.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ / /{index}");
    }
    /// <summary>
    /// It's a function that searches for a movie in the database and returns the result
    /// </summary>
    private async Task Search()
    {
        index = 0;
        if (!string.IsNullOrWhiteSpace(searchString))
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

    /// <summary>
    /// It loads more movies from the database and adds them to the list of movies
    /// </summary>
    private async Task LoadMore()
    {
        index++;
        if (isSearch)
        {
            movies?.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/{searchString}//{index}"));
        }
        else if (sort != null)
        {
            movies?.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ /{sort}/{index}"));
        }
        else
        {
            movies?.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ / /{index}"));
        }
    }
}
