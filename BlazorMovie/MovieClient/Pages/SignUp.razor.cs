using BlazorMovie.Shared;
using MovieClient.Services;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SignUp
{
    private string role = "Customer";
    private readonly ShowAlertService alertService = new();
    private void Role()
    {
        if (role == "Customer")
        {
            role = "Studio";
        }
        else
        {
            role = "Customer";
        }
    }

    private readonly RegisterModel signUpModel = new()
    { DateOfBirth = DateTime.Now };
    protected async Task HandleValidSubmit()
    {
        if (signUpModel.ConfirmPassword != signUpModel.Password)
        {
            alertService.ShowAlert(Severity.Warning, "Password and Confirm Password are different");
            return;
        }
        signUpModel.Role = role;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Account/Register", signUpModel);
        alertService.ShowAlert(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        _accountService.checkAuthentication();
        _navigationManager.NavigateTo("/");
    }

}