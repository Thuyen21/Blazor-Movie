using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class StatusStudio
    {
        [Parameter]
        public string Id { get; set; }

        protected List<CommentModel> commentList = new();
        protected List<int> commentStatus = new();
        protected string content;
        protected override async Task OnInitializedAsync()
        {
            //commentList = await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Studio/Comment/{Id}");
            commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}");
        }

        protected async Task Submit()
        {
            content = "checking";
            commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/check");
            _navigationManager.NavigateTo($"/StatusStudio/{Id}");
        }
    }
}