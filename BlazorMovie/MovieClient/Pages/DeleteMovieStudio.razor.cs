using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using MovieClient.Services;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class DeleteMovieStudio
{
    [Parameter]
    public string? Id { get; set; }

    private MovieModel? movie;
    private readonly ShowAlertService alertService = new();
    protected override async Task OnInitializedAsync()
    {
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}");
    }

    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Studio/DeleteMovie", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}
