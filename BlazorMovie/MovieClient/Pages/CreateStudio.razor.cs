using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components.Forms;
using MovieClient.Services;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class CreateStudio
{
    private readonly MovieModel movie = new();
    
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Studio/MovieUpload", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    protected override void OnInitialized()
    {
        movie.PremiereDate = DateTime.Now;
    }
}
