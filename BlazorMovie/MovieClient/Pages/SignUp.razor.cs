using BlazorMovie.Shared;
using MudBlazor;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class SignUp
{
    private string role = "Customer";
    private bool showAlert = false;
    private Severity severity;
    private void CloseAlert()
    {
        showAlert = false;
    }

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

    private readonly SignUpModel signUpModel = new()
    { DateOfBirth = DateTime.Now };
    private string content;
    protected async Task HandleValidSubmit()
    {
        signUpModel.Role = role;
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("user/signUp", signUpModel);
        content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            severity = Severity.Success;
            showAlert = true;
            _navigationManager.NavigateTo("/", true);
        }
        else
        {
            severity = Severity.Error;
            showAlert = true;
        }
    }

}