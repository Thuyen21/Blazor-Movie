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
    public partial class SalaryStudio
    {
        protected string Email;
        protected string EmailConfirm;
        protected double Cash;
        protected string content;
        protected async Task Submit()
        {
            try
            {
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(Email);
            }
            catch
            {
                content = "Not Email";
                return;
            }

            if (Email != EmailConfirm)
            {
                content = "Check your email";
            }
            else
            {
                content = await (await _httpClient.PostAsJsonAsync<Dictionary<string, string>>("Studio/Salary", new Dictionary<string, string>()
                {{"Email", Email}, {"Cash", Cash.ToString()}})).Content.ReadAsStringAsync();
            }
        }
    }
}