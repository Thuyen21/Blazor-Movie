using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class DeleteMovieStudio
{
    /* A parameter that is passed in from the parent component. */
    [Parameter]
    public string? Id { get; set; }

    /* A private variable that is used to store the movie data that is returned from the API. */
    private MovieModel? movie;

    /// <summary>
    /// The function is called when the page is initialized. It gets the movie data from the API and
    /// stores it in the movie variable
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Studio/EditMovie/{Id}");
    }

    /// <summary>
    /// It sends a POST request to the server with the movie object as the body of the request.
    /// 
    /// The response is then passed to the alert service to display a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// 
    /// The alert service is a simple service that displays a message to the user.
    /// </summary>
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Studio/DeleteMovie", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}
