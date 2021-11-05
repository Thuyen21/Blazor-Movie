using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class WatchCustomer
{
    [Parameter]
#pragma warning disable CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable field 'movie' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private MovieModel movie;
#pragma warning restore CS8618 // Non-nullable field 'movie' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string? movieLink = null;
    private bool? canWatch;
#pragma warning disable CS8618 // Non-nullable field 'Acomment' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string Acomment;
#pragma warning restore CS8618 // Non-nullable field 'Acomment' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string content;
#pragma warning restore CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private List<CommentModel> commentList = new();
    private bool showAlert = false;
    private Severity severity;
    private bool sameDevice = false;
    private bool firstPlay = true;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private int index = 0;
    protected override async Task OnInitializedAsync()
    {
        Task? movieTask = Task.Run(async () =>
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            movie = await _httpClient.GetFromJsonAsync<MovieModel>($"Customer/Watch/{Id}");
#pragma warning restore CS8601 // Possible null reference assignment.
        });
        Task? commentTask = Task.Run(async () =>
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            commentList = await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}");
#pragma warning restore CS8601 // Possible null reference assignment.
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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            tokena = await _httpClient.GetFromJsonAsync<char[]>("User/GetToken");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        });
        string token = new string(tokena);
        try
        {
            await Task.WhenAll(movieTask, commentTask, sameDeviceTask, canWatchTask, tokenaTask);
        }
        catch
        {
            content = "Turn off AdBlock";
            severity = Severity.Error;
            showAlert = true;
            return;
        }

        if (canWatch == true)
        {
            try
            {
                movieLink = await new FirebaseStorage("movie2-e3c7b.appspot.com", new FirebaseStorageOptions { AuthTokenAsyncFactory = async () => await Task.FromResult(await Task.FromResult(token)), ThrowOnCancel = true, HttpClientTimeout = TimeSpan.FromHours(2) }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie").GetDownloadUrlAsync();
            }
            catch (Exception)
            {
                content = "This studio's movie file contains a mistake.";
                severity = Severity.Error;
                showAlert = true;
            }
        }
    }

    private async Task View()
    {
        if (firstPlay)
        {
            await _httpClient.PostAsJsonAsync("Customer/View", Id);
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
                content = "Turn off AdBlock";
                showAlert = true;
                severity = Severity.Error;
                StateHasChanged();
                return;
            }

            if (!sameDevice)
            {
                content = "You cannot watch in many Device at the same time Logout then Login to update your last Device";
                showAlert = true;
                severity = Severity.Error;
                StateHasChanged();
            }

            await Task.Delay(60000);
        }
    }

    private async Task Com()
    {
        CommentModel up = new CommentModel()
        { Time = DateTime.UtcNow, MovieId = Id, CommentText = Acomment };
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("Customer/Acomment", up);
        content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            severity = Severity.Success;
        }
        else
        {
            severity = Severity.Error;
        }

        showAlert = true;
        List<CommentModel> commentListTemp = new();
        for (int i = 0; i <= index; i++)
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{i}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
        }

        commentList.Clear();
        commentList = commentListTemp;
        StateHasChanged();
    }

    private async Task AcDisLike(string Id)
    {
        await _httpClient.PostAsJsonAsync($"Customer/ac/{Id}", "DisLike");
        List<CommentModel> commentListTemp = new();
        for (int i = 0; i <= index; i++)
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{this.Id}/{i}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
        }

        commentList.Clear();
        commentList = commentListTemp;
        StateHasChanged();
    }

    private async Task AcLike(string Id)
    {
        await _httpClient.PostAsJsonAsync($"Customer/ac/{Id}", "Like");
        List<CommentModel> commentListTemp = new();
        for (int i = 0; i <= index; i++)
        {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{this.Id}/{i}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
        }

        commentList.Clear();
        commentList = commentListTemp;
        StateHasChanged();
    }

    private async Task LoadMore()
    {
        index++;
#pragma warning disable CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
        commentList.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}"));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'collection' in 'void List<CommentModel>.AddRange(IEnumerable<CommentModel> collection)'.
    }
}
