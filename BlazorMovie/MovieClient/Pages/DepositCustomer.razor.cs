using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class DepositCustomer
{
    private string content;
    private double cash = 0;
    private string clientToken;
    private string divCSS = "display: none;";
    private void DivCSS(string divCSS)
    {
        this.divCSS = divCSS;
    }

    private DotNetObjectReference<DepositCustomer> objRef;
    private bool doneCash = false;
    private async Task Recharge()
    {
        content = null;
        doneCash = false;
        cash = 0;
        DivCSS("display: none;");
    }

    private async Task Cash()
    {
        try
        {
            doneCash = true;
            clientToken = new string(await _httpClient.GetFromJsonAsync<char[]>("Customer/Deposit"));
            objRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("Deposit", objRef, clientToken, cash);
            DivCSS("display: block;");
        }
        catch (Exception)
        {
        }
    }

    [JSInvokable]
    public async Task Test(string test, string test2, string test3)
    {
        HttpResponseMessage? response = await _httpClient.PostAsJsonAsync($"Customer/{test3}", new List<string>()
            {test, test2});
        content = await response.Content.ReadAsStringAsync();
        StateHasChanged();
    }
}
