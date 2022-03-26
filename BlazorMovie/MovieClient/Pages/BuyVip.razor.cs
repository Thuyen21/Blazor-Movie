using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class BuyVip
{
    [Parameter]
    public string? movieId { get; set; }

    private readonly VipModel vip = new()
    { Choose = 1 };
    private string? vipStatus;


    protected override async Task OnInitializedAsync()
    {
        if (movieId != null)
        {
            vip.Id = movieId;
            vip.Choose = 0;
        }

        vipStatus = new string(await _httpClient.GetFromJsonAsync<char[]>("Customer/VipCheck"));
    }

    private async Task Buy()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Customer/BuyVip", vip);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        vipStatus = new string(await _httpClient.GetFromJsonAsync<char[]>("Customer/VipCheck"));
        StateHasChanged();
    }
}
