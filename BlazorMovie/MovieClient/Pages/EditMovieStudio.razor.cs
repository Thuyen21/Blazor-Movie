using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MovieClient.Services;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditMovieStudio
{
    [Parameter]
    public string? Id { get; set; }
    private MovieModel? movie;
    private IBrowserFile? movieFile;
    private IBrowserFile? imageFile;
    private string? mp;
    private string? ip;
    private ShowAlertService alertService = new();
    private bool more = false;
    protected override async Task OnInitializedAsync()
    {
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}");
    }

    private async Task HandleValidSubmit()
    {
        MovieModel moviePost = await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}");
        movie.StudioId = moviePost.StudioId;
        movie.MovieId = moviePost.MovieId;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Studio/EditMovie", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    private async Task OnChooseMovieFile(InputFileChangeEventArgs e)
    {
        List<string> list = new List<string> { "video/x-msvideo", "video/mp4", "video/mpeg", "video/ogg", "video/mp2t", "video/webm", "video/3gpp", "video/3gpp2", "video/x-matroska" };
        if (list.Contains(e.File.ContentType))
        {
            movieFile = e.File;
            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken")!;
            string token = new string(tokena);

            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").PutAsync(movieFile.OpenReadStream(long.MaxValue));
            task.Progress.ProgressChanged += (s, e) =>
            {
                mp = e.Percentage.ToString() + "%";
                StateHasChanged();
            };
            try
            {
                await task;
            }
            catch
            {
                alertService.ShowAlert(Severity.Error, "More 500MB use the other method upload");
            }

        }
        else
        {
            alertService.ShowAlert(Severity.Error, "Incorrect MIME");
        }
    }

    private async Task OnChooseImageFile(InputFileChangeEventArgs e)
    {
        List<string> list = new List<string> { "image/bmp", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/svg+xml", "image/tiff", "image/webp" };
        if (list.Contains(e.File.ContentType))
        {
            imageFile = e.File;
            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
            string token = new string(tokena);
            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Image").PutAsync(imageFile.OpenReadStream(long.MaxValue));
            task.Progress.ProgressChanged += (s, e) =>
            {
                ip = e.Percentage.ToString() + "%";
                StateHasChanged();
            };
            try
            {
                await task;
            }
            catch
            {
                alertService.ShowAlert(Severity.Error, "More 500MB use the other method upload");
            }
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
