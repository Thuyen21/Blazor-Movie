using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditMovieAdmin
{
    /* A parameter that is passed to the component. */
    [Parameter]
    public string? Id { get; set; }

    /* Initializing the movie variable to a new instance of the MovieModel class. */
    private MovieModel movie = new();
    /* A variable that is used to store the file that is uploaded. */
    private IBrowserFile? movieFile;
    /* A variable that is used to store the file that is uploaded. */
    private IBrowserFile? imageFile;
    /* A variable that is used to store the file that is uploaded. */
    private string? mp;
    /* A variable that is used to store the file that is uploaded. */
    private string? ip;
    /* A variable that is used to store the file that is uploaded. */
    private bool more = false;

    /// <summary>
    /// The function is called when the component is initialized. It gets the movie from the database
    /// and assigns it to the movie variable
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        movie = (await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}"))!;
    }

    /// <summary>
    /// It gets the movie from the database, sets the movie id and studio id, then posts the movie to
    /// the database.
    /// </summary>
    private async Task HandleValidSubmit()
    {

        MovieModel moviePost = (await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}"))!;
        movie.StudioId = moviePost.StudioId;
        movie.MovieId = moviePost.MovieId;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/EditMovie", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// It's a function that uploads a video file to Firebase Storage
    /// </summary>
    /// <param name="InputFileChangeEventArgs">The event arguments for the file input change
    /// event.</param>
    private async Task OnChooseMovieFile(InputFileChangeEventArgs e)
    {
        List<string> list = new() { "video/x-msvideo", "video/mp4", "video/mpeg", "video/ogg", "video/mp2t", "video/webm", "video/3gpp", "video/3gpp2", "video/x-matroska" };
        if (list.Contains(e.File.ContentType))
        {
            movieFile = e.File;
            char[] tokena = (await _httpClient.GetFromJsonAsync<char[]>("User/GetToken"))!;
            string token = new(tokena);
            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").PutAsync(movieFile.OpenReadStream(long.MaxValue));
            task.Progress.ProgressChanged += (s, e) =>
            {

                mp = e.Percentage.ToString() + "%";
                StateHasChanged();
            };
            try
            {
                _ = await task;
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

    /// <summary>
    /// It takes a file from the user, uploads it to Firebase Storage, and then displays the progress of
    /// the upload
    /// </summary>
    /// <param name="InputFileChangeEventArgs">The event arguments for the input file change
    /// event.</param>
    private async Task OnChooseImageFile(InputFileChangeEventArgs e)
    {
        List<string> list = new() { "image/bmp", "image/gif", "image/jpg", "image/jpeg", "image/png", "image/svg+xml", "image/tiff", "image/webp" };
        if (list.Contains(e.File.ContentType))
        {
            imageFile = e.File;

            char[] tokena = (await _httpClient.GetFromJsonAsync<char[]>("User/GetToken"))!;
            string token = new(tokena);

            FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Image").PutAsync(imageFile.OpenReadStream(long.MaxValue));
            task.Progress.ProgressChanged += (s, e) =>
            {
                ip = e.Percentage.ToString() + "%";
                StateHasChanged();
            };
            try
            {
                _ = await task;
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

    /// <summary>
    /// It sets the variable more to true.
    /// </summary>
    private void Upload()
    {
        more = true;
    }
}
