using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class ReadFeedBack
{
    /* Creating a new instance of the `List<FeedbackMessageModel>` class. */
    private readonly List<FeedbackMessageModel> feedbacks = new();
    /* The page number. */
    private int index = 1;
    /// <summary>
    /// It gets the feedbacks from the database and adds them to the feedbacks list.
    /// </summary>
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

    /// <summary>
    /// It loads more feedbacks from the server and adds them to the list of feedbacks
    /// </summary>
    private async Task LoadMore()
    {
        index++;
        feedbacks.AddRange(await _httpClient.GetFromJsonAsync<List<FeedbackMessageModel>>($"admin/ReadFeedBack/{index}"));
    }

}