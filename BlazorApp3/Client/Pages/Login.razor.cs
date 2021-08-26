using BlazorApp3.Shared;
using Microsoft.JSInterop;
using System.Net.Http.Json;
namespace BlazorApp3.Client.Pages
{
	public partial class Login
	{
		protected LogInModel login = new LogInModel();
		protected string content;


		protected async Task HandleValidSubmit()
		{
			var remoteUserAgent = await JS.InvokeAsync<string>("getUserAgent");
			

            
				login.UserAgent = remoteUserAgent;

			HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LogInModel>("user/login", login);
			content = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
			{

				//_navigationManager.NavigateTo("/", true);
				_accountService.Login();
				//new CustomAuthenticationStateProvider(_httpClient).Update();
				//new NavMenu().MenuChanged();
				_navigationManager.NavigateTo("/");
			}
		}
	}
}