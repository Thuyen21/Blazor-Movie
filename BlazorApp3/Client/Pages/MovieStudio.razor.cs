using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class MovieStudio
    {
        protected List<MovieModel> movies = new();
        protected string NameSort = "name";
        protected string DateSort = "date";
        protected string GenreSort = "genre";
        protected string sortOrder = "Id";
        protected string searchString { get; set; }

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
                    movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>("Studio/Index/ /{no}");
                }
                else
                {
                    movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sortOrder}");
                }
            }
            else
            {
                movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/{searchString}/{sortOrder}");
            }
        }
    }
}