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
        movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>("user/Trending");
        Parallel.ForEach(movies, async item =>
        {
            DicImageLink.Add(item.MovieId, null);
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
