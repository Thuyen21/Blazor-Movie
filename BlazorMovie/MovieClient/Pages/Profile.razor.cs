using BlazorMovie.Shared;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class Profile
{
    /* Creating a new instance of the ChangeEmailModel class. */
    private readonly ChangeEmailModel changeEmail = new();

    /* Creating a new instance of the AccountManagementModel class. */
    private AccountManagementModel accountManagementModel = new();
    /// <summary>
    /// It sends a POST request to the server with the new email address, and if the request is
    /// successful, it updates the accountManagementModel with the new email address
    /// </summary>
    private async Task ChangeEmail()
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync("User/ChangeEmail", changeEmail);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        _accountService.checkAuthentication();
        accountManagementModel = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/Profile") ?? new();
    }

    /// <summary>
    /// It navigates to the ResetPassword page
    /// </summary>
    /// <returns>
    /// A Task object.
    /// </returns>
    private Task ChangePass()
    {
        _navigationManager.NavigateTo("/ResetPassword");
        return Task.CompletedTask;
    }
    /// <summary>
    /// The function is called when the page is initialized. It calls the API and gets the data from the
    /// API and stores it in the accountManagementModel variable
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        accountManagementModel = await _httpClient.GetFromJsonAsync<AccountManagementModel>("user/Profile") ?? new();
    }

    /// <summary>
    /// It sends a POST request to the server with the data from the form, and then shows an alert based
    /// on the response.
    /// 
    /// The `PostAsJsonAsync` method is a helper method that serializes the object into JSON and sends
    /// it to the server.
    /// 
    /// The `alertService` is a service that shows an alert based on the response.
    /// 
    /// The `accountManagementModel` is the model that contains the data from the form.
    /// 
    /// The `IsSuccessStatusCode` property is a boolean that indicates whether the request was
    /// successful.
    /// 
    /// The `ReadAsStringAsync` method is a helper method that reads the response as a string.
    /// 
    /// The `ShowAlert` method is a method that shows an alert based on the response.
    /// 
    /// The `await` keyword is used to wait for the response.
    /// 
    /// The `async` keyword
    /// </summary>
    private async Task HandleValidSubmit()
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/EditProfile", accountManagementModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
    }

}