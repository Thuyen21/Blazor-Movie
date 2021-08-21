using BlazorApp3.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using MauiApp1;
using MauiApp1.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using MauiApp1.Services;
namespace MauiApp1.Pages
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