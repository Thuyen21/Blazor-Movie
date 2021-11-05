using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class ReadFeedBack
{
    private readonly List<FeedbackMessageModel> feedbacks = new();
    private int index = 1;
    protected override async Task OnInitializedAsync()
    {
        try
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<FeedbackMessageModel>.AddRange(IEnumerable<FeedbackMessageModel> collection)'.
            feedbacks.AddRange(await _httpClient.GetFromJsonAsync<List<FeedbackMessageModel>>($"admin/ReadFeedBack/{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<FeedbackMessageModel>.AddRange(IEnumerable<FeedbackMessageModel> collection)'.
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task LoadMore()
    {
        index++;
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<FeedbackMessageModel>.AddRange(IEnumerable<FeedbackMessageModel> collection)'.
        feedbacks.AddRange(await _httpClient.GetFromJsonAsync<List<FeedbackMessageModel>>($"admin/ReadFeedBack/{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<FeedbackMessageModel>.AddRange(IEnumerable<FeedbackMessageModel> collection)'.
    }

}