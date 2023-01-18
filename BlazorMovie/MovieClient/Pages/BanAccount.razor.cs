using BlazorMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class BanAccount
{
    /* A parameter that is passed in from the parent component. */
    [Parameter]
    public string? Id { get; set; }
    /* Creating a new instance of the AccountManagementModel class. */
    private AccountManagementModel acc = new();

   /// <summary>
   /// The function is called when the page is initialized. It gets the account information from the
   /// database and stores it in the acc variable
   /// </summary>
    protected override async Task OnInitializedAsync()
    {
        acc = (await _httpClient.GetFromJsonAsync<AccountManagementModel>($"Admin/EditAccount/{Id}"))!;
    }

   /// <summary>
   /// It sends a POST request to the server with the account id as the body of the request
   /// </summary>
    private async Task Ban()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/Ban", acc.Id);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// It sends a POST request to the server with the account id as the body of the request
    /// </summary>
    private async Task UnBan()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Admin/UnBan", acc.Id);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }
}
