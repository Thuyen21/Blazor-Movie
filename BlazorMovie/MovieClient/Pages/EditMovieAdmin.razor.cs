using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditMovieAdmin
{
    [Parameter]
    public string? Id { get; set; }

    private MovieModel? movie;
    private IBrowserFile? movieFile;
    private IBrowserFile? imageFile;
    private string? content;
    private string? mp;
    private string? ip;
    private bool more = false;
    private string? linkIframe;
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    protected override async Task OnInitializedAsync()
    {
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}");
        linkIframe = $"{_httpClient.BaseAddress}Admin/MovieUpload/{Id}";
    }

    private async Task HandleValidSubmit()
    {

        MovieModel moviePost = await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}");
        movie.StudioId = moviePost.StudioId;
        movie.MovieId = moviePost.MovieId;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/EditMovie", movie);
        content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            severity = Severity.Success;
        }
        else
        {
            severity = Severity.Error;
        }

        showAlert = true;
    }

    private async Task OnChooseMovieFile(InputFileChangeEventArgs e)
    {
        List<string> list = new List<string> { "video/x-msvideo", "video/mp4", "video/mpeg", "video/ogg", "video/mp2t", "video/webm", "video/3gpp", "video/3gpp2", "video/x-matroska" };
        if (list.Contains(e.File.ContentType))
        {
            movieFile = e.File;
            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
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
                content = "More 500MB use the other method upload";
                severity = Severity.Error;
                showAlert = true;
            }
        }
        else
        {
            content = "Incorrect MIME";
            severity = Severity.Error;
            showAlert = true;
        }
    }

    private async Task OnChooseImageFile(InputFileChangeEventArgs e)
    {
        List<string> list = new List<string> { "image/bmp", "image/gif", "image/jpg", "image/jpeg", "image/png", "image/svg+xml", "image/tiff", "image/webp" };
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
                content = "More 500MB use the other method upload";
                severity = Severity.Error;
                showAlert = true;
            }
        }
        else
        {
            content = "Incorrect MIME";
            severity = Severity.Error;
            showAlert = true;
        }
    }

    private void Upload()
    {
        more = true;
    }
}
