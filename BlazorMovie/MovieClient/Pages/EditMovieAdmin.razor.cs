using BlazorMovie.Shared.Movie;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MovieClient.Services;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditMovieAdmin
{
    [Parameter]
    public string? Id { get; set; }
    private MovieInputModel movie = new();
    private string? mp;
    private string? ip;
    private bool more = false;
    
    protected override async Task OnInitializedAsync()
    {
        movie = (await _httpClient.GetFromJsonAsync<MovieInputModel>($"Admin/EditMovie/{Id}"))!;
    }

    private async Task HandleValidSubmit()
    {

        MovieInputModel moviePost = (await _httpClient.GetFromJsonAsync<MovieInputModel>($"Admin/EditMovie/{Id}"))!;
        movie.StudioId = moviePost.StudioId;
        movie.Id = moviePost.Id;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/EditMovie", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    private async Task OnChooseMovieFile(InputFileChangeEventArgs e)
    {
        List<string> list = new List<string> { "video/x-msvideo", "video/mp4", "video/mpeg", "video/ogg", "video/mp2t", "video/webm", "video/3gpp", "video/3gpp2", "video/x-matroska" };
        if (list.Contains(e.File.ContentType))
        {
            movie.MovieFile = new byte[e.File.Size];
            await e.File.OpenReadStream(movie.MovieFile.Length).ReadAsync(movie.MovieFile);
            movie.MovieFileExtensions = e.File.ContentType;

        }
        else
        {
            alertService.ShowAlert(Severity.Error, "Incorrect MIME");
        }
    }

    private async Task OnChooseImageFile(InputFileChangeEventArgs e)
    {
        List<string> list = new List<string> { "image/bmp", "image/gif", "image/jpg", "image/jpeg", "image/png", "image/svg+xml", "image/tiff", "image/webp" };
        if (list.Contains(e.File.ContentType))
        {
            movie.ImageFile = new byte[e.File.Size];
            await e.File.OpenReadStream(movie.ImageFile.Length).ReadAsync(movie.ImageFile);
            movie.ImageFileExtensions = e.File.ContentType;

        }
        else
        {
            alertService.ShowAlert(Severity.Error, "Incorrect MIME");
        }
    }

    private void Upload()
    {
        more = true;
    }
}
