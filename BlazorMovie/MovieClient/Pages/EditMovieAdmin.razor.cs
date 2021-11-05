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
#pragma warning disable CS0649 // Field 'EditMovieAdmin.mp' is never assigned to, and will always have its default value null
#pragma warning disable CS8618 // Non-nullable field 'mp' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private readonly string mp;
#pragma warning restore CS8618 // Non-nullable field 'mp' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning restore CS0649 // Field 'EditMovieAdmin.mp' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'EditMovieAdmin.ip' is never assigned to, and will always have its default value null
#pragma warning disable CS8618 // Non-nullable field 'ip' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private readonly string ip;
#pragma warning restore CS8618 // Non-nullable field 'ip' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning restore CS0649 // Field 'EditMovieAdmin.ip' is never assigned to, and will always have its default value null
    private bool more = false;
#pragma warning disable CS8618 // Non-nullable field 'linkIframe' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string linkIframe;
#pragma warning restore CS8618 // Non-nullable field 'linkIframe' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    protected override async Task OnInitializedAsync()
    {
#pragma warning disable CS8601 // Possible null reference assignment.
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}");
#pragma warning restore CS8601 // Possible null reference assignment.
        linkIframe = $"{_httpClient.BaseAddress}Admin/MovieUpload/{Id}";
    }

    private async Task HandleValidSubmit()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        MovieModel moviePost = await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        movie.StudioId = moviePost.StudioId;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string token = new string(tokena);
            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").PutAsync(movieFile.OpenReadStream(long.MaxValue));
            task.Progress.ProgressChanged += (s, e) =>
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
        List<string> list = new List<string> { "image/bmp", "image/gif", "image/jpg", "image/jpeg", "image/png", "image/svg+xml", "image/tiff", "image/webp" };
        if (list.Contains(e.File.ContentType))
        {
            imageFile = e.File;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string token = new string(tokena);
            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Image").PutAsync(imageFile.OpenReadStream(long.MaxValue));
            task.Progress.ProgressChanged += (s, e) =>
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

    private void Upload()
    {
        more = true;
    }
}
