using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using BlazorApp3.Client;
using BlazorApp3.Shared;
using BlazorApp3.Client.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BlazorApp3.Client.Services;
using Braintree;

namespace BlazorApp3.Client.Pages
{
    public partial class DepositCustomer
    {
        [Parameter]
        public string Code { get; set; }

        private double cash = 0;
        private string clientToken;
        protected string divCSS = "display: none;";
        protected void DivCSS(string divCSS)
        {
            this.divCSS = divCSS;
        }

        private async Task Cash()
        {
            try
            {
                clientToken = new string (await _httpClient.GetFromJsonAsync<char[]>("Customer/Deposit"));
                await JS.InvokeVoidAsync("Deposit", clientToken, cash);
                DivCSS("display: block;");
            }
            catch (Exception ex)
            {
            }
        }
    }
}