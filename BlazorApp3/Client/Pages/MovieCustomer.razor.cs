using BlazorApp3.Shared;
using Firebase.Storage;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class MovieCustomer
    {
        protected List<MovieModel> movies = new();
        protected string? searchString { get; set; }
        protected bool isSearch = false;
        protected Dictionary<string, string> DicImageLink = new();
        protected string sort = null;
        protected int index = 0;
        protected async Task NameSortParm()
        {
            index = 0;
            sort = sort == "name" ? "nameDesc" : "name";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        protected async Task DateSortParm()
        {
            index = 0;
            sort = sort == "date" ? "dateDesc" : "date";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        protected async Task GenreSortParm()
        {
            index = 0;
            sort = sort == "genre" ? "genreDesc" : "genre";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        protected override async Task OnInitializedAsync()
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Customer/Movie/ / /{index}");

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
        protected async Task Search()
        {
            index = 0;
            if (searchString != null)
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
        }
        protected async Task LoadMore()
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

        }
    }
}