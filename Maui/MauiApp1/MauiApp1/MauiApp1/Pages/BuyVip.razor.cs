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
    public partial class BuyVip
    {
        [Parameter]
        public string movieId { get; set; }

        protected VipModel vip = new VipModel()
        { Choose = 1 };
        protected string vipStatus;
        protected string content;
        protected override async Task OnInitializedAsync()
        {
            if (movieId != null)
            {
                vip.Id = movieId;
                vip.Choose = 0;
            }

            vipStatus = new string(await _httpClient.GetFromJsonAsync<char[]>("Customer/VipCheck"));
        }

        protected async Task Buy()
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<VipModel>("Customer/BuyVip", vip);
            content = await response.Content.ReadAsStringAsync();
            await OnInitializedAsync();
        }
    }
}