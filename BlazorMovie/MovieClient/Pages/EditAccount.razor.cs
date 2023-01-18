using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class EditAccount
{
    /* A parameter that is passed in from the parent component. */
    [Parameter]
    public string? Id { get; set; }

    /* Creating a new instance of the AccountManagementModel class. */
    private AccountManagementModel acc = new();

    /// <summary>
    /// It gets the account information from the database and stores it in the acc variable.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        acc = (await _httpClient.GetFromJsonAsync<AccountManagementModel>($"Admin/EditAccount/{Id}"))!;
    }

    /// <summary>
    /// It takes the account object, changes the date of birth to the next day, and then sends it to the
    /// server.
    /// 
    /// The problem is that the date of birth is not changing.
    /// 
    /// I've tried changing the date of birth to the next day in the following ways:
    /// 
    /// 1. accNew.DateOfBirth = accNew.DateOfBirth.AddDays(1);
    /// 2. accNew.DateOfBirth = accNew.DateOfBirth.ToUniversalTime().AddDays(1);
    /// 3. accNew.DateOfBirth = accNew.DateOfBirth.ToLocalTime().AddDays(1);
    /// 4. accNew.DateOfBirth = accNew.DateOfBirth.ToLocalTime().AddDays(1).ToUniversalTime();
    /// 
    /// None of these work.
    /// 
    /// I've also tried changing the date of birth to the
    /// </summary>
    private async Task HandleValidSubmit()
    {
        AccountManagementModel accNew = acc;
        accNew.DateOfBirth = accNew.DateOfBirth.ToUniversalTime().AddDays(1);
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/EditAccount", accNew);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}
