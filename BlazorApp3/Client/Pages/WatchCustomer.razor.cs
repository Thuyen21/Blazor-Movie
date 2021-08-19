using BlazorApp3.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class WatchCustomer
    {
        [Parameter]
        public string Id { get; set; }

        protected MovieModel movie;
        protected string movieLink;
        protected bool? canWatch;
        protected string Acomment;
        protected string content;
        protected List<CommentModel> commentList = new();
        
        protected int index = 0;
        protected override async Task OnInitializedAsync()
        {
            if (Id == null)
            {
                _navigationManager.NavigateTo("/MovieAdmin");
            }

            movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Customer/Watch/{Id}");
            canWatch = await _httpClient.GetFromJsonAsync<bool>($"Customer/CanWatch/{Id}");
            commentList = await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}");
            if (canWatch == true)
            {
                char[] tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
                string token = new string(tokena);
                try
                {
                    movieLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").GetDownloadUrlAsync();
                }
                catch
                {
                }
            }
        }

        protected async Task Com()
        {
            CommentModel up = new CommentModel()
            { Time = DateTime.UtcNow, MovieId = Id, CommentText = Acomment };
            content = await (await _httpClient.PostAsJsonAsync<CommentModel>("Customer/Acomment", up)).Content.ReadAsStringAsync();
            commentList = await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}");
            
        }

        protected async Task AcDisLike(string Id)
        {
            await _httpClient.PostAsJsonAsync<string>($"Customer/ac/{Id}", "DisLike");
            await OnInitializedAsync();
        }

        protected async Task AcLike(string Id)
        {
            await _httpClient.PostAsJsonAsync<string>($"Customer/ac/{Id}", "Like");
            await OnInitializedAsync();
        }

        protected async Task LoadMore()
        {
            index++;
            commentList.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}"));
        }
    }
}