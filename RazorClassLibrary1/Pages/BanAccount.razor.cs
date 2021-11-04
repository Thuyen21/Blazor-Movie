using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class BanAccount
{
    [Parameter]
    public string Id { get; set; }

    private AccountManagementModel acc;
    private string content;
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

    protected override async Task OnInitializedAsync()
    {
        acc = await _httpClient.GetFromJsonAsync<AccountManagementModel>($"Admin/EditAccount/{Id}");
    }

    private async Task Ban()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<string>("Admin/Ban", acc.Id);
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

    private async Task UnBan()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<string>("Admin/UnBan", acc.Id);
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
}
