using BlazorMovie.Shared;
using Firebase.Storage;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class MovieCustomer
{
    private List<MovieModel> movies = new();
    private string searchString = string.Empty;
    private bool isSearch = false;
    private readonly Dictionary<string, string> DicImageLink = new();
    private string? sort = null;
    private int index = 0;
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }

    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }

    private async Task GenreSortParm()
    {
        index = 0;
        sort = sort == "genre" ? "genreDesc" : "genre";
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}");
        isSearch = false;
        searchString = string.Empty;
        await LoadImg();
    }
    private string? token;
    protected override async Task OnInitializedAsync()
    {
        Task? moviesTask = Task.Run(async () =>
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ / /{index}");
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
    private Task LoadImg()
    {
        _ = Parallel.ForEach(movies, async item =>
        {
            try
            {
                if (!DicImageLink.ContainsKey(item.MovieId))
                {
                    string ImageLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true }).Child(item.StudioId).Child(item.MovieId).Child("Image").GetDownloadUrlAsync();
                    DicImageLink.Add(item.MovieId, ImageLink);
                    StateHasChanged();
                }
            }
            catch
            {
            }
        });
        return Task.CompletedTask;
    }

    private async Task Search()
    {
        index = 0;
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/{searchString}/ /{index}");
            isSearch = true;
        }
        else
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ / /{index}");
            isSearch = false;
        }

        sort = null;
        await LoadImg();
    }

    private async Task LoadMore()
    {
        index++;
        if (isSearch)
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/{searchString}//{index}"));
        }
        else if (sort != null)
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}"));
        }
        else
        {
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ / /{index}"));
        }
        await LoadImg();
    }
}
