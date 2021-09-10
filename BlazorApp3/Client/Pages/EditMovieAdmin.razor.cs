using BlazorApp3.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class EditMovieAdmin
    {
        [Parameter]
        public string Id { get; set; }

        protected MovieModel movie;
        protected IBrowserFile movieFile;
        protected IBrowserFile imageFile;
        protected string content;
        protected string mp;
        protected string ip;
        protected override async Task OnInitializedAsync()
        {
            if (Id == null)
            {
                _navigationManager.NavigateTo("/MovieAdmin");
            }

            movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}");
        }

        protected async Task HandleValidSubmit()
        {
            MovieModel moviePost = await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}");
            movie.StudioId = moviePost.StudioId;
            movie.MovieId = moviePost.MovieId;
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<MovieModel>("Admin/EditMovie", movie);
            content = await response.Content.ReadAsStringAsync();
        }

        protected async Task OnChooseMovieFile(InputFileChangeEventArgs e)
        {
            List<string> list = new List<string>
            {
                "video/x-msvideo",
                "video/mp4",
                "video/mpeg",
                "video/ogg",
                "video/mp2t",
                "video/webm",
                "video/3gpp",
                "video/3gpp2",
                "video/x-matroska"
            };
            if (list.Contains(e.File.ContentType))
            {
                movieFile = e.File;
                char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
                string token = new string(tokena);
                FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").PutAsync(movieFile.OpenReadStream(long.MaxValue));
                task.Progress.ProgressChanged += async (s, e) =>
                {
                    content = e.Percentage.ToString();
                };
                try
                {
                    await task;
                }
                catch
                {
                    content = "More 500MB use the other method upload";
                }
            }
            else
            {
                content = "Incorrect MIME";
            }
        }

        protected async Task OnChooseImageFile(InputFileChangeEventArgs e)
        {
            List<string> list = new List<string>
            {
                "image/bmp",
                "image/gif",
                "image/jpeg",
                "image/png",
                "image/svg+xml",
                "image/tiff",
                "image/webp"
            };
            if (list.Contains(e.File.ContentType))
            {
                imageFile = (e.File);
                char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
                string token = new string(tokena);
                FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Image").PutAsync(imageFile.OpenReadStream(long.MaxValue));
                task.Progress.ProgressChanged += async (s, e) =>
                {
                    content = e.Percentage.ToString();
                };
                try
                {
                    await task;
                }
                catch
                {
                    content = "More 500MB use the other method upload";
                }

            }
            else
            {
                content = "Incorrect MIME";
            }
        }

        public async Task Upload()
        {
            _navigationManager.NavigateTo($"/admin/MovieUpload/{Id}", true);
        }
    }
}