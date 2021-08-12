using Microsoft.AspNetCore.Components;

namespace BlazorApp3.Client.Pages
{
    public partial class DepositCustomer
    {
        [Parameter]
        public string Code { get; set; }

        protected string content;
        protected override async Task OnInitializedAsync()
        {
            if (Code == null)
            {
                _navigationManager.NavigateTo("/Customer/Deposit", true);
            }
            else
            {
                content = Code;
            }
        }
    }
}