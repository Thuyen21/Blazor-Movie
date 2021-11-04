using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Contact
{
    private readonly FeedbackMessageModel feedbackMessageModel = new();
    private string? content;
    private bool showAlert = false;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task SendFeedbackMessage()
    {
        feedbackMessageModel.Time = DateTime.UtcNow;
        content = await (await _httpClient.PostAsJsonAsync("user/feedback", feedbackMessageModel)).Content.ReadAsStringAsync();
        showAlert = true;
    }
}
