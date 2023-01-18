using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class BuyVip
{
   /* A parameter that is passed from the parent component. */
    [Parameter]
    public string? movieId { get; set; }

    /* Creating a new instance of the VipModel class. */
    private readonly VipModel vip = new() { Choose = 1 };
    /* A variable that is used to store the response from the server. */
    private string? vipStatus;
    /// <summary>
    /// It checks if the user is a VIP or not.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        if (movieId != null)
        {
            vip.Id = movieId;
            vip.Choose = 0;
        }

        vipStatus = new string(await _httpClient.GetFromJsonAsync<char[]>("Customer/VipCheck"));
    }

    /// <summary>
    /// It sends a post request to the server with the vip object as the body, then it shows an alert
    /// based on the response, then it gets the vip status from the server and updates the page
    /// </summary>
    private async Task Buy()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Customer/BuyVip", vip);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        vipStatus = new string(await _httpClient.GetFromJsonAsync<char[]>("Customer/VipCheck"));
        StateHasChanged();
    }
}
