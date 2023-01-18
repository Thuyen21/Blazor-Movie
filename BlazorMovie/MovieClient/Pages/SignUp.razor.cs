using BlazorMovie.Shared;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SignUp
{
    /* Setting the default value of the role to Customer. */
    private string role = "Customer";

    /// <summary>
    /// If the role is "Customer", then set the role to "Studio". Otherwise, set the role to "Customer"
    /// </summary>
    private void Role()
    {
        role = role == "Customer" ? "Studio" : "Customer";
    }

    /* Creating a new instance of the SignUpModel class and setting the DateOfBirth property to the
    current date. */
    private readonly SignUpModel signUpModel = new()
    { DateOfBirth = DateTime.Now };
    /// <summary>
    /// It sends a POST request to the server with the signUpModel as the body. If the response is
    /// successful, it shows an alert and navigates to the home page.
    /// </summary>
    /// <returns>
    /// The response from the server.
    /// </returns>
    protected async Task HandleValidSubmit()
    {
        if (signUpModel.ConfirmPassword != signUpModel.Password)
        {
            alertService.ShowAlert(Severity.Warning, "Password and Confirm Password are different");
            return;
        }
        signUpModel.Role = role;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/signUp", signUpModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        _accountService.checkAuthentication();
        _navigationManager.NavigateTo("/");
    }

}