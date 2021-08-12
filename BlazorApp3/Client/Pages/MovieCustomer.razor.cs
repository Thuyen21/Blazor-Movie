using BlazorApp3.Shared;
using Firebase.Storage;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class MovieCustomer
    {
        protected List<MovieModel> movies = new();
        protected string NameSort = "name";
        protected string DateSort = "date";
        protected string GenreSort = "genre";
        protected string sortOrder = "Id";
        protected string searchString { get; set; }

        protected Dictionary<string, string> DicImageLink = new();
        protected async Task NameSortParm()
        {
            sortOrder = NameSort;
            NameSort = NameSort == "name" ? "nameDesc" : "name";
            searchString = " ";
            await OnInitializedAsync();
        }

        protected async Task DateSortParm()
        {
            sortOrder = DateSort;
            DateSort = DateSort == "date" ? "dateDesc" : "date";
            searchString = " ";
            await OnInitializedAsync();
        }

        protected async Task GenreSortParm()
        {
            sortOrder = GenreSort;
            GenreSort = GenreSort == "genre" ? "genreDesc" : "genre";
            searchString = " ";
            await OnInitializedAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(searchString))
            {
                if (string.IsNullOrEmpty(sortOrder))
                {
                    movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>("Customer/Movie/ /{no}");
                }
                else
                {
                    movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sortOrder}");
                }
            }
            else
            {
                movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/{searchString}/{sortOrder}");
            }

            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
            string token = new string(tokena);
            foreach (MovieModel item in movies)
            {
                try
                {
                    string ImageLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(item.StudioId).Child(item.MovieId).Child("Image").GetDownloadUrlAsync();
                    if (!DicImageLink.ContainsKey(item.MovieId))
                    {
                        DicImageLink.Add(item.MovieId, ImageLink);
                    }
                    else
                    {
                        DicImageLink[item.MovieId] = ImageLink;
                    }
                }
                catch
                {
                    DicImageLink.Add(item.MovieId, null);
                }
            }
        }
    }
}