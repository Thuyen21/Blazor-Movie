using BlazorMovie.Shared;
using BlazorMovie.Shared.Account;
using Microsoft.AspNetCore.Components;
using MovieClient.Services;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class BanAccount
{
    [Parameter]
    public string? Id { get; set; }
    private UserViewModel acc = new();
    
    protected override async Task OnInitializedAsync()
    {
        acc = (await _httpClient.GetFromJsonAsync<UserViewModel>($"api/Admin/GetUserById?Id={Id}"))!;
    }

    private async Task Ban()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/Ban", acc.Id);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    private async Task UnBan()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Admin/UnBan", acc.Id);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());

    }
}
