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
    public partial class CreateStudio
    {
        protected MovieModel movie = new();
        protected string content;
        protected async Task HandleValidSubmit()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<MovieModel>("Studio/Upload", movie);
            content = await response.Content.ReadAsStringAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            movie.PremiereDate = DateTime.Now;
        }
    }
}