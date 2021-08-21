using BlazorApp3.Shared;
using System.Net.Http.Json;
using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using MauiApp1;
using MauiApp1.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using MauiApp1.Services;
namespace MauiApp1.Pages
{
    public partial class SignUp
    {
        private string role = "Customer";
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
        protected SignUpModel signUpModel = new()
        { DateOfBirth = DateTime.Now };
        protected string content;
        protected async Task HandleValidSubmit()
        {
            signUpModel.Role = role;
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<SignUpModel>("user/signUp", signUpModel);
            if (response.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("/", true);
            }
            else
            {
                content = await response.Content.ReadAsStringAsync();
            }
        }
    }
}