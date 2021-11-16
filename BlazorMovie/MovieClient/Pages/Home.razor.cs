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
#pragma warning disable CS8601 // Possible null reference assignment.
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>("user/Trending");
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument for parameter 'source' in 'ParallelLoopResult Parallel.ForEach<MovieModel>(IEnumerable<MovieModel> source, Action<MovieModel> body)'.
        Parallel.ForEach(movies, async item =>
#pragma warning restore CS8604 // Possible null reference argument for parameter 'source' in 'ParallelLoopResult Parallel.ForEach<MovieModel>(IEnumerable<MovieModel> source, Action<MovieModel> body)'.
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument for parameter 'key' in 'void Dictionary<string, string>.Add(string key, string value)'.
            DicImageLink.Add(item.MovieId, null);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'key' in 'void Dictionary<string, string>.Add(string key, string value)'.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            try
            {
                string ImageLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(item.StudioId).Child(item.MovieId).Child("Image").GetDownloadUrlAsync();
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
        try
        {
            return DicImageLink.ElementAt(index).Value;
        }
        catch
        {
            return "https://via.placeholder.com/768x512/FFA07A/000000";
        }
    }
}
