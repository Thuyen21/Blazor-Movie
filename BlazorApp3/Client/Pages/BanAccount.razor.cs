using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
	public partial class BanAccount
	{
		[Parameter]
		public string Id { get; set; }

		protected AccountManagementModel acc;
		protected string content;
		protected override async Task OnInitializedAsync()
		{
			if (Id == null)
			{
				_navigationManager.NavigateTo("/AdminAccountManagement");
			}

			acc = await _httpClient.GetFromJsonAsync<AccountManagementModel>($"Admin/EditAccount/{Id}");
		}

		protected async Task Ban()
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync<string>("Admin/Ban", acc.Id);
			content = await response.Content.ReadAsStringAsync();
		}

		protected async Task UnBan()
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync<string>("Admin/UnBan", acc.Id);
			content = await response.Content.ReadAsStringAsync();
		}
	}
}