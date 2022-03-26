using BlazorMovie.Shared;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SignUp
{
    private string role = "Customer";

    private void Role()
    {
        role = role == "Customer" ? "Studio" : "Customer";
    }

    private readonly SignUpModel signUpModel = new()
    { DateOfBirth = DateTime.Now };
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