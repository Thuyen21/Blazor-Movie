using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

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
        content = await (await _httpClient.PostAsJsonAsync("user/feedback", feedbackMessageModel)).Content.ReadAsStringAsync();
        showAlert = true;
    }
}
