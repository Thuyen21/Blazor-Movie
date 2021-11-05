using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditMovieStudio
{
    [Parameter]
#pragma warning disable CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable field 'movie' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private MovieModel movie;
#pragma warning restore CS8618 // Non-nullable field 'movie' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'movieFile' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private IBrowserFile movieFile;
#pragma warning restore CS8618 // Non-nullable field 'movieFile' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'imageFile' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private IBrowserFile imageFile;
#pragma warning restore CS8618 // Non-nullable field 'imageFile' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string content;
#pragma warning restore CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'linkUp' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string linkUp;
#pragma warning restore CS8618 // Non-nullable field 'linkUp' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'linkIframe' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string linkIframe;
#pragma warning restore CS8618 // Non-nullable field 'linkIframe' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private bool more = false;
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    protected override async Task OnInitializedAsync()
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}");
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        linkUp = $"/Studio/MovieUpload/{movie.MovieId}";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        linkIframe = $"{_httpClient.BaseAddress}Studio/MovieUpload/{Id}";
    }

    private async Task HandleValidSubmit()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        MovieModel moviePost = await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        movie.StudioId = moviePost.StudioId;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        movie.MovieId = moviePost.MovieId;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Studio/EditMovie", movie);
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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string token = new string(tokena);
            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").PutAsync(movieFile.OpenReadStream(long.MaxValue));
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            task.Progress.ProgressChanged += async (s, e) =>
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            {
                content = e.Percentage.ToString();
                severity = Severity.Info;
                showAlert = true;
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
        List<string> list = new List<string> { "image/bmp", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/svg+xml", "image/tiff", "image/webp" };
        if (list.Contains(e.File.ContentType))
        {
            imageFile = e.File;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string token = new string(tokena);
            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Image").PutAsync(imageFile.OpenReadStream(long.MaxValue));
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            task.Progress.ProgressChanged += async (s, e) =>
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
            {
                content = e.Percentage.ToString();
                severity = Severity.Info;
                showAlert = true;
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

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    private async Task Upload()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    {
        more = true;
    }
}
