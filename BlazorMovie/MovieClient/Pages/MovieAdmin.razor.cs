using BlazorMovie.Shared.Movie;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class MovieAdmin
{
    private List<MovieViewModel>? movies = new();
    private int index = 0;
    private string searchString = string.Empty;
    private string sort = string.Empty;
    private async Task NameSortParm()
    {
        index = 0;
        sort = sort == "name" ? "nameDesc" : "name";
        movies = (await _httpClient.GetFromJsonAsync<List<MovieViewModel>>($"api/Admin/Movie?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    private async Task DateSortParm()
    {
        index = 0;
        sort = sort == "date" ? "dateDesc" : "date";
        movies = (await _httpClient.GetFromJsonAsync<List<MovieViewModel>>($"api/Admin/Movie?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    private async Task GenreSortParm()
    {
        index = 0;
        sort = sort == "genre" ? "genreDesc" : "genre";
        movies = (await _httpClient.GetFromJsonAsync<List<MovieViewModel>>($"api/Admin/Movie?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    protected override async Task OnInitializedAsync()
    {
        movies = (await _httpClient.GetFromJsonAsync<List<MovieViewModel>>($"api/Admin/Movie?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }
    private async Task Search()
    {
        index = 0;
        movies = (await _httpClient.GetFromJsonAsync<List<MovieViewModel>>($"api/Admin/Movie?searchString={searchString}&orderBy={sort}&index={index}"))!;
    }

    private async Task LoadMore()
    {
        index++;
        movies.AddRange((await _httpClient.GetFromJsonAsync<List<MovieViewModel>>($"api/Admin/Movie?searchString={searchString}&orderBy={sort}&index={index}"))!);
    }
}
