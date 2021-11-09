using BlazorMovie.Shared;
using Firebase.Storage;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class MovieStudio
{
    private List<MovieModel> movies = new();
    private int index = 0;
    private string? searchString { get; set; }

    private bool isSearch = false;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    private string sort = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    private readonly Dictionary<string, string> DicImageLink = new();
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
#pragma warning disable CS8601 // Possible null reference assignment.
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
        isSearch = false;
        searchString = null;
        await LoadImg();
    }

    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
#pragma warning disable CS8601 // Possible null reference assignment.
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
        isSearch = false;
        searchString = null;
        await LoadImg();
    }
    private string token;
    private async Task GenreSortParm()
    {
        index = 0;
        sort = sort == "genre" ? "genreDesc" : "genre";
#pragma warning disable CS8601 // Possible null reference assignment.
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
        isSearch = false;
        searchString = null;
        await LoadImg();
    }

    protected override async Task OnInitializedAsync()
    {
        Task? moviesTask = Task.Run(async () =>
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
        });
        char[] tokena = { };
        Task? tokenaTask = Task.Run(async () =>
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        });
        await Task.WhenAll(moviesTask, tokenaTask);
        token = new string(tokena);
        await LoadImg();
    }
    private async Task LoadImg()
    {
        Parallel.ForEach(movies, async item =>
        {
            try
            {
                if (!DicImageLink.ContainsKey(item.MovieId))
                {
                    string ImageLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true}).Child(item.StudioId).Child(item.MovieId).Child("Image").GetDownloadUrlAsync();
                    DicImageLink.Add(item.MovieId, ImageLink);

                    StateHasChanged();
                }
            }
            catch
            {
               
            }
        });
    }
    private async Task Search()
    {
        index = 0;
        if (searchString != null)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/{searchString}/ /{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
            isSearch = true;
        }
        else
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
            isSearch = false;
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        sort = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        await LoadImg();
    }

    private async Task LoadMore()
    {
        index++;
        if (isSearch)
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<MovieModel>.AddRange(IEnumerable<MovieModel> collection)'.
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/{searchString}//{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<MovieModel>.AddRange(IEnumerable<MovieModel> collection)'.
        }
        else if (sort != null)
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<MovieModel>.AddRange(IEnumerable<MovieModel> collection)'.
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<MovieModel>.AddRange(IEnumerable<MovieModel> collection)'.
        }
        else
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<MovieModel>.AddRange(IEnumerable<MovieModel> collection)'.
            movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<MovieModel>.AddRange(IEnumerable<MovieModel> collection)'.
        }
        await LoadImg();
    }
}
