
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp3.Client.Services
{
	public class AccountService : IAccountService
	{
		private readonly AuthenticationStateProvider _authenticationStateProvider;
		public AccountService(AuthenticationStateProvider authenticationStateProvider)
		{
			_authenticationStateProvider = authenticationStateProvider;
		}
		public bool Login()
		{
			(_authenticationStateProvider as CustomAuthenticationStateProvider).Update();
			return true;
		}

		public bool Logout()
		{
			(_authenticationStateProvider as CustomAuthenticationStateProvider).Update();
			return true;
		}
	}
}

