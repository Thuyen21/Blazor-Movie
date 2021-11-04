using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class EditAccount
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

    private async Task HandleValidSubmit()
    {
        AccountManagementModel accNew = acc;
        accNew.DateOfBirth = accNew.DateOfBirth.ToUniversalTime().AddDays(1);
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<AccountManagementModel>("Admin/EditAccount", accNew);
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
