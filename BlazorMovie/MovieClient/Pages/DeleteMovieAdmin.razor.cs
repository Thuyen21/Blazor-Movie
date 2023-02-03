using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class DeleteMovieAdmin
{
    /* A parameter that is passed to the component. */
    [Parameter]
    public string? Id { get; set; }
    /* A private variable that is used to store the movie data. */
    private MovieModel? movie;

    /// <summary>
    /// The function is called when the component is initialized. It gets the movie from the database
    /// and stores it in the movie variable
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Admin/EditMovie/{Id}");
    }
    /// <summary>
    /// It sends a POST request to the server with the movie object as the body of the request.
    /// 
    /// The response is then passed to the alert service to display a message to the user.
    /// 
    /// The alert service is a service that I created to display messages to the user.
    /// 
    /// The alert service is a service that I created to display messages to the user.
    /// 
    /// The alert service is a service that I created to display messages to the user.
    /// 
    /// The alert service is a service that I created to display messages to the user.
    /// 
    /// The alert service is a service that I created to display messages to the user.
    /// 
    /// The alert service is a service that I created to display messages to the user.
    /// 
    /// The alert service is a service that I created to display messages to the user.
    /// 
    /// The alert service is a service that I created to display
    /// </summary>
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/DeleteMovie", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}
