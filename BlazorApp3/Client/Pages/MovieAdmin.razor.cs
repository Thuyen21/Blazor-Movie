using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class MovieAdmin
    {
        protected List<MovieModel> movies = new();
        protected int index = 0;
        protected string? searchString { get; set; }
        protected bool isSearch = false;

        protected string sort = null;

        protected async Task NameSortParm()
        {
            index = 0;
            sort = sort == "name" ? "nameDesc" : "name";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
            isSearch = false;
            searchString = null;

        }

        protected async Task DateSortParm()
        {
            index = 0;
            sort = sort == "date" ? "dateDesc" : "date";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
            isSearch = false;
            searchString = null;

        }

        protected async Task GenreSortParm()
        {
            index = 0;
            sort = sort == "genre" ? "genreDesc" : "genre";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Admin/Movie/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        protected override async Task OnInitializedAsync()
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ / /{index}");

        }
        protected async Task Search()
        {
            index = 0;
            if (searchString != null)
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
        protected async Task LoadMore()
        {
            index++;
            if (isSearch)
            {
                movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/{searchString}//{index}"));
            }
            else if (sort != null)
            {
                movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ /{sort}/{index}"));
            }
            else
            {
                movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"admin/Movie/ / /{index}"));
            }

        }
    }
}