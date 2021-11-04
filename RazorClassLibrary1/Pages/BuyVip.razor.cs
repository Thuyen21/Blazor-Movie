using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class BuyVip
{
    [Parameter]
    public string movieId { get; set; }

    private readonly VipModel vip = new VipModel()
    { Choose = 1 };
    private string vipStatus;
    private string content;
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
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<VipModel>("Customer/BuyVip", vip);
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
