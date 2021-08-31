using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class DeleteMovieAdmin
    {
        [Parameter]
        public string Id { get; set; }

        protected MovieModel movie;
        protected string content;
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
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<MovieModel>("Admin/DeleteMovie", movie);
            content = await response.Content.ReadAsStringAsync();
        }
    }
}