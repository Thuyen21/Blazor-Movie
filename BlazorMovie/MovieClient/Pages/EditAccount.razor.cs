using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditAccount
{
    [Parameter]
#pragma warning disable CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable field 'acc' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private AccountManagementModel acc;
#pragma warning restore CS8618 // Non-nullable field 'acc' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
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
#pragma warning disable CS8601 // Possible null reference assignment.
        acc = await _httpClient.GetFromJsonAsync<AccountManagementModel>($"Admin/EditAccount/{Id}");
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    private async Task HandleValidSubmit()
    {
        AccountManagementModel accNew = acc;
        accNew.DateOfBirth = accNew.DateOfBirth.ToUniversalTime().AddDays(1);
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/EditAccount", accNew);
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
