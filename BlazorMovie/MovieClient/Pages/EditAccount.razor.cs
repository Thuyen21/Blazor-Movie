using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditAccount
{
    [Parameter]
    public string? Id { get; set; }

    private AccountManagementModel acc = new();

    protected override async Task OnInitializedAsync()
    {
        acc = (await _httpClient.GetFromJsonAsync<AccountManagementModel>($"Admin/EditAccount/{Id}"))!;
    }

    private async Task HandleValidSubmit()
    {
        AccountManagementModel accNew = acc;
        accNew.DateOfBirth = accNew.DateOfBirth.ToUniversalTime().AddDays(1);
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/EditAccount", accNew);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}
