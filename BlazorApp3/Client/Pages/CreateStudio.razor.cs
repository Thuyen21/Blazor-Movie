using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class CreateStudio
    {
        protected MovieModel movie = new();
        protected string content;
        protected async Task HandleValidSubmit()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<MovieModel>("Studio/Upload", movie);
            content = await response.Content.ReadAsStringAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            movie.PremiereDate = DateTime.Now;
        }
    }
}