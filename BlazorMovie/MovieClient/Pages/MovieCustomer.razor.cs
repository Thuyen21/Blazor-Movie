using BlazorMovie.Shared;
using Firebase.Storage;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class MovieCustomer
{
    /* Creating a new instance of the list. */
    private List<MovieModel> movies = new();
    /* A variable that is used to store the search string. */
    private string searchString = string.Empty;
    /* A variable that is used to store the search string. */
    private bool isSearch = false;
    /* A dictionary that is used to store the image links. */
    private readonly Dictionary<string, string> DicImageLink = new();
    /* A variable that is used to store the sort string. */
    private string? sort = null;
    /* Used to store the index of the page. */
    private int index = 0;
    /// <summary>
    /// This function is used to sort the movies by name
    /// </summary>
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}") ?? new();
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }

    /// <summary>
    /// It's a function that sorts the movies by date
    /// </summary>
    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}") ?? new();
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }

    /// <summary>
    /// This function is used to sort the movies by genre
    /// </summary>
    private async Task GenreSortParm()
    {
        index = 0;
        sort = sort == "genre" ? "genreDesc" : "genre";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}") ?? new();
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }
    /* Used to store the token. */
    private string? token;
    /// <summary>
    /// It gets a list of movies from the server, and then gets a token from the server.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        Task? moviesTask = Task.Run(async () =>
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ / /{index}") ?? new();
        });
        char[] tokena = { };
        Task? tokenaTask = Task.Run(async () =>
        {
            tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
        });
        await Task.WhenAll(moviesTask, tokenaTask);
        token = new string(tokena);
        await LoadImg();
    }
    /// <summary>
    /// It's a function that loads images from firebase storage and stores them in a dictionary
    /// </summary>
    /// <returns>
    /// A Task.
    /// </returns>
    private Task LoadImg()
    {
        _ = Parallel.ForEach(movies, async item =>
        {

            if (!DicImageLink.ContainsKey(item.MovieId))
            {
                string ImageLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true }).Child(item.StudioId).Child(item.MovieId).Child("Image").GetDownloadUrlAsync();
                DicImageLink.Add(item.MovieId, ImageLink);
                StateHasChanged();
            }

        });
        return Task.CompletedTask;
    }

    /// <summary>
    /// It gets a list of movies from the server and loads the images for each movie
    /// </summary>
    private async Task Search()
    {
        index = 0;
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/{searchString}/ /{index}") ?? new();
            isSearch = true;
        }
        else
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ / /{index}") ?? new();
            isSearch = false;
        }

        sort = null;
        await LoadImg();
    }

    /// <summary>
    /// It loads more movies from the database and adds them to the list of movies
    /// </summary>
    private async Task LoadMore()
    {
        index++;
        if (isSearch)
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/{searchString}//{index}") ?? new());
        }
        else if (sort != null)
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}") ?? new());
        }
        else
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ / /{index}") ?? new());
        }
        await LoadImg();
    }
}
