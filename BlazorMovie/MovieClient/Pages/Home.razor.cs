using BlazorMovie.Shared;
using Firebase.Storage;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Home
{
    private List<MovieModel> movies = new();
    private readonly Dictionary<string, string> DicImageLink = new();
    protected override async Task OnInitializedAsync()
    {
        Task? movieTask = Task.Run(async () =>
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>("user/Trending");
        });
        char[] tokena = { };
        Task? tokenaTask = Task.Run(async () =>
        {
            tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
        });
        await Task.WhenAll(movieTask, tokenaTask);
        string token = new(tokena);

        _ = Parallel.ForEach(movies, async item =>
        {
            DicImageLink.Add(item.MovieId, null);
            try
            {
                string ImageLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(token), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(item.StudioId).Child(item.MovieId).Child("Image").GetDownloadUrlAsync();
                if (!DicImageLink.ContainsKey(item.MovieId))
                {
                    DicImageLink.Add(item.MovieId, ImageLink);
                    StateHasChanged();
                }
                else
                {
                    DicImageLink[item.MovieId] = ImageLink;
                    StateHasChanged();
                }
            }
            catch
            {
            }
        });
    }
    private string getLink(int index)
    {
        List<string> img = new() { "D3D3D3", "FFB6C1", "87CEFA", "B0C4DE", "20B2AA", "FFA07A" };
        Random? random = new();
        try
        {
            return DicImageLink.ElementAt(index).Value;
        }
        catch
        {
            return $"https://via.placeholder.com/768x512/{img[random.Next(img.Count)]}/000000";
        }
    }
}
