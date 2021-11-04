using BlazorApp3.Shared;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class ReadFeedBack
{
    private readonly List<FeedbackMessageModel> feedbacks = new();
    private int index = 1;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            feedbacks.AddRange(await _httpClient.GetFromJsonAsync<List<FeedbackMessageModel>>($"admin/ReadFeedBack/{index}"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task LoadMore()
    {
        index++;
        feedbacks.AddRange(await _httpClient.GetFromJsonAsync<List<FeedbackMessageModel>>($"admin/ReadFeedBack/{index}"));
    }

}