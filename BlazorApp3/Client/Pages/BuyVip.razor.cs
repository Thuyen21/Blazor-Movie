using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
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