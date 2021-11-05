using BlazorMovie.Shared;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class CreateStudio
{
    private readonly MovieModel movie = new();
    private string? content;
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Studio/Upload", movie);
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
    }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    {
        movie.PremiereDate = DateTime.Now;
    }
}
