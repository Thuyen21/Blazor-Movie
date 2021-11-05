using Microsoft.AspNetCore.Components;
using MovieClient.Shared;

namespace MovieClient.Pages;

public partial class Logout
{
    [CascadingParameter]
#pragma warning disable CS8618 // Non-nullable property 'Error' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public Error Error { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Error' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    protected override async Task OnInitializedAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("user/Logout");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Reason: {response.ReasonPhrase}, Message: {content}");
        }

        _accountService.Logout();
        _navigationManager.NavigateTo("/");
    }
}
