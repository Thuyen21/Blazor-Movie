using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class BuyVip
{
    [Parameter]
#pragma warning disable CS8618 // Non-nullable property 'movieId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string movieId { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'movieId' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    private readonly VipModel vip = new VipModel()
    { Choose = 1 };
#pragma warning disable CS8618 // Non-nullable field 'vipStatus' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string vipStatus;
#pragma warning restore CS8618 // Non-nullable field 'vipStatus' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
#pragma warning disable CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string content;
#pragma warning restore CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

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
        vipStatus = new string(await _httpClient.GetFromJsonAsync<char[]>("Customer/VipCheck"));
        StateHasChanged();
    }
}
