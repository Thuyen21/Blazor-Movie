using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Contact
{
    /* Creating a new instance of the FeedbackMessageModel class. */
    private readonly FeedbackMessageModel feedbackMessageModel = new();
    /// <summary>
    /// It sends a feedback message to the server.
    /// </summary>
    private async Task SendFeedbackMessage()
    {
        feedbackMessageModel.Time = DateTime.UtcNow;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/feedback", feedbackMessageModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}
