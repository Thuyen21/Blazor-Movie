using BlazorMovie.Shared;
using Firebase.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MovieClient.Services;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class WatchCustomer
{
    [Parameter]
    public string? Id { get; set; }

    private MovieModel? movie;
    private string? movieLink = null;
    private bool? canWatch;
    private string? Acomment;
    private List<CommentModel> commentList = new();
    private bool sameDevice = false;
    private bool firstPlay = true;
    
    private int index = 0;
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
        string token = new string(tokena);

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

    private async Task Com()
    {
        CommentModel up = new CommentModel()
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

    private async Task AcDisLike(string Id)
    {
        await _httpClient.PostAsJsonAsync($"Customer/ac/{Id}", "DisLike");
        List<CommentModel> commentListTemp = new();
        for (int i = 0; i <= index; i++)
        {
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{this.Id}/{i}"));
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
            commentListTemp.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{this.Id}/{i}"));
        }

        commentList.Clear();
        commentList = commentListTemp;
        StateHasChanged();
    }

    private async Task LoadMore()
    {
        index++;
        commentList.AddRange(await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Customer/Comment/{Id}/{index}"));
    }
}
