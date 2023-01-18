using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class CreateStudio
{
    /* Creating a new instance of the MovieModel class. */
    private readonly MovieModel movie = new();

    /// <summary>
    /// It takes the movie object and sends it to the server using the PostAsJsonAsync method.
    /// 
    /// The PostAsJsonAsync method is a method of the HttpClient class. It takes two parameters: the URL
    /// of the server and the object to be sent.
    /// 
    /// The URL of the server is the URL of the server followed by the name of the controller and the
    /// name of the action. In this case, the URL is "Studio/Upload".
    /// 
    /// The object to be sent is the movie object.
    /// 
    /// The PostAsJsonAsync method returns an HttpResponseMessage object. The HttpResponseMessage object
    /// has a property called IsSuccessStatusCode. This property is a boolean. It is true if the server
    /// successfully received the movie object. It is false if the server did not successfully receive
    /// the movie object.
    /// 
    /// The HttpResponseMessage object also has a property called Content
    /// </summary>
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Studio/Upload", movie);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// The OnInitialized() function is called when the component is initialized
    /// </summary>
    protected override void OnInitialized()
    {
        movie.PremiereDate = DateTime.Now;
    }
}
