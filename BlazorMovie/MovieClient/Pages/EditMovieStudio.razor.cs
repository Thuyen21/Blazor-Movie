using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditMovieStudio
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
    /* A variable that is used to store the progress of the upload. */
    private string? mp;
    /* A variable that is used to store the progress of the upload. */
    private string? ip;

    /* A variable that is used to show the upload button. */
    private bool more = false;
    /// <summary>
    /// The function is called when the page is initialized. It gets the movie data from the API and
    /// stores it in the movie variable.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}") ?? new();
    }

    /// <summary>
    /// It gets the movie from the database, sets the movie's id and studio id, and then posts the movie
    /// to the database.
    /// </summary>
    private async Task HandleValidSubmit()
    {
        MovieModel moviePost = (await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}"))!;
        movie.StudioId = moviePost.StudioId;
        movie.MovieId = moviePost.MovieId;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Studio/EditMovie", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// It takes a file from the user, checks if it's a video, then uploads it to Firebase Storage
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
    /// The function is called when the user selects a file to upload. The function checks the file type
    /// and if it is an image, it uploads the image to Firebase Storage
    /// </summary>
    /// <param name="InputFileChangeEventArgs">The event arguments for the input file change
    /// event.</param>
    private async Task OnChooseImageFile(InputFileChangeEventArgs e)
    {
        List<string> list = new() { "image/bmp", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/svg+xml", "image/tiff", "image/webp" };
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
