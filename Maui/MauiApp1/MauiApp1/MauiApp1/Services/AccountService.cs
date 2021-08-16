using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
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
