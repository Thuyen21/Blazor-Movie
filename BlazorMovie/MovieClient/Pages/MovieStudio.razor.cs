using BlazorMovie.Shared;
using Firebase.Storage;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class MovieStudio
{
    /* Creating a new instance of the list. */
    private List<MovieModel> movies = new();
    /* Used to load more movies. */
    private int index = 0;
    /* A variable that is used to store the search string. */
    private string searchString = string.Empty;
    /* Used to check if the user is searching for a movie. */
    private bool isSearch = false;
    /* Used to sort the movies by name, date, or genre. */
    private string? sort = null;
    /* A dictionary that is used to store the image links. */
    private readonly Dictionary<string?, string> DicImageLink = new();
    /// <summary>
    /// This function is called when the user clicks on the Name column header. It sorts the movies by
    /// name
    /// </summary>
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}") ?? new();
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
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}") ?? new();
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }
    /* Used to store the token that is used to access the Firebase Storage. */
    private string? token;
    /// <summary>
    /// This function is called when the user clicks on the Genre column header. It sets the sort
    /// parameter to genre or genreDesc, depending on the current sort parameter. It then calls the
    /// Index function in the Studio controller, passing in the sort parameter and the index parameter.
    /// The Index function returns a list of movies, which is then assigned to the movies variable. The
    /// isSearch variable is set to false, the searchString variable is set to an empty string, and the
    /// LoadImg function is called
    /// </summary>
    private async Task GenreSortParm()
    {
        index = 0;
        sort = sort == "genre" ? "genreDesc" : "genre";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}") ?? new();
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }
    /// <summary>
    /// It gets a list of movies from the server, and then gets a token from the server, and then loads
    /// the images for the movies
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        Task? moviesTask = Task.Run(async () =>
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}") ?? new();
        });
        char[] tokena = { };
        Task? tokenaTask = Task.Run(async () =>
        {
            tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken") ?? [];
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
    /// It's a function that gets a list of movies from the server and loads the images of the movies.
    /// </summary>
    private async Task Search()
    {
        index = 0;
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/{searchString}/ /{index}") ?? new();
            isSearch = true;
        }
        else
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}") ?? new();
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
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/{searchString}//{index}") ?? new());
        }
        else if (sort != null)
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}") ?? new());
        }
        else
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}") ?? new());
        }
        await LoadImg();
    }
}
