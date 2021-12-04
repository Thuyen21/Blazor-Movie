using BlazorMovie.Shared;
using MovieClient.Services;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Contact
{
    private readonly FeedbackMessageModel feedbackMessageModel = new();
    private ShowAlertService alertService = new();

    private async Task SendFeedbackMessage()
    {
        feedbackMessageModel.Time = DateTime.UtcNow;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/feedback", feedbackMessageModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());

    }
}
