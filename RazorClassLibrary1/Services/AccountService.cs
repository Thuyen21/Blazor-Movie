using Microsoft.AspNetCore.Components.Authorization;
using RazorClassLibrary1;

namespace RazorClassLibrary1.Services
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (_authenticationStateProvider as CustomAuthenticationStateProvider).Update();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return true;
        }

        public bool Logout()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (_authenticationStateProvider as CustomAuthenticationStateProvider).Update();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return true;
        }

    }
}

