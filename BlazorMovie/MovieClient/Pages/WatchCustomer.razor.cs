using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class WatchCustomer
{
    /* A parameter that is passed to the component. */
    [Parameter]
    public string? Id { get; set; }

    /* A private field that is used to store the movie model. */
    private MovieModel? movie;
    /* A private field that is used to store the movie link. */
    private string? movieLink = null;
    /* A private field that is used to store the movie link. */
    private bool? canWatch;
    /* A private field that is used to store the comment text. */
    private string? Acomment;
    /* Creating a new instance of the `List<CommentModel>` class. */
    private List<CommentModel> commentList = new();
    /* Used to check if the user is watching the movie on the same device. */
    private bool sameDevice = false;
    /* Used to check if the user is watching the movie on the same device. */
    private bool firstPlay = true;

    /* Used to load more comments. */
    private int index = 0;
    /// <summary>
    /// It gets the movie, comments, and whether the user can watch the movie
    /// </summary>
    /// <returns>
    /// The movie object, the comment list, the sameDevice bool, the canWatch bool, and the token char
    /// array.
    /// </returns>
    protected override async Task OnInitializedAsync()
    {
        Task? movieTask = Task.Run(async () =>
        {
            movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Customer/Watch/{Id}");
        });
        Task? commentTask = Task.Run(async () =>
        {
            commentList = await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}");
        });
        Task? sameDeviceTask = Task.Run(async () =>
        {
            sameDevice = (await _httpClient.PostAsJsonAsync("Customer/UserAgent", await JS.InvokeAsync<string>("getUserAgent"))).IsSuccessStatusCode;
        });
        Task? canWatchTask = Task.Run(async () =>
        {
            canWatch = await _httpClient.GetFromJsonAsync<bool>($"Customer/CanWatch/{Id}");
            Console.WriteLine("Done");
        });
        char[] tokena = { };
        Task? tokenaTask = Task.Run(async () =>
        {
            tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
        });

        try
        {
            await Task.WhenAll(movieTask, commentTask, sameDeviceTask, canWatchTask, tokenaTask);

        }
        catch
        {
            alertService.ShowAlert(Severity.Error, "Turn off AdBlock");
            return;
        }
        string token = new(tokena);

        if (canWatch == true)
        {
            try
            {
                movieLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(token), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").GetDownloadUrlAsync();
            }
            catch (Exception)
            {
                alertService.ShowAlert(Severity.Error, "This studio's movie file contains a mistake.");
            }
        }
    }
    /// <summary>
    /// It's a function that runs every minute and checks if the user is watching the video from the
    /// same device
    /// </summary>
    /// <returns>
    /// The return value is a Task.
    /// </returns>
    private async Task View()
    {
        if (firstPlay)
        {
            _ = await _httpClient.PostAsJsonAsync("Customer/View", Id);
            firstPlay = false;
        }

        while (true)
        {
            try
            {
                sameDevice = (await _httpClient.PostAsJsonAsync("Customer/UserAgent", await JS.InvokeAsync<string>("getUserAgent"))).IsSuccessStatusCode;
            }
            catch
            {
                alertService.ShowAlert(Severity.Error, "Turn off AdBlock");
                StateHasChanged();
                return;
            }

            if (!sameDevice)
            {
                alertService.ShowAlert(Severity.Error, "You cannot watch in many Device at the same time Logout then Login to update your last Device");
                StateHasChanged();
            }
            await Task.Delay(60000);
        }
    }

    /// <summary>
    /// It sends a comment to the server, then it gets the comments from the server and puts them in a
    /// list
    /// </summary>
    private async Task Com()
    {
        CommentModel up = new()
        { Time = DateTime.UtcNow, MovieId = Id, CommentText = Acomment };
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Customer/Acomment", up);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        List<CommentModel> commentListTemp = new();
        for (int i = 0; i <= index; i++)
        {
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{i}"));
        }

        commentList.Clear();
        commentList = commentListTemp;
        StateHasChanged();
    }

    /// <summary>
    /// It sends a post request to the server and then gets the updated list of comments.
    /// </summary>
    /// <param name="Id">The id of the comment</param>
    private async Task AcDisLike(string Id)
    {
        _ = await _httpClient.PostAsJsonAsync($"Customer/ac/{Id}", "DisLike");
        List<CommentModel> commentListTemp = new();
        for (int i = 0; i <= index; i++)
        {
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{this.Id}/{i}"));
        }

        commentList.Clear();
        commentList = commentListTemp;
        StateHasChanged();
    }

    /// <summary>
    /// It sends a post request to the server, then gets the updated list of comments from the server,
    /// and then updates the list of comments in the client
    /// </summary>
    /// <param name="Id">The id of the comment</param>
    private async Task AcLike(string Id)
    {
        _ = await _httpClient.PostAsJsonAsync($"Customer/ac/{Id}", "Like");
        List<CommentModel> commentListTemp = new();
        for (int i = 0; i <= index; i++)
        {
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{this.Id}/{i}"));
        }

        commentList.Clear();
        commentList = commentListTemp;
        StateHasChanged();
    }

    /// <summary>
    /// It adds the next page of comments to the list of comments
    /// </summary>
    private async Task LoadMore()
    {
        index++;
        commentList.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}"));
    }
}
