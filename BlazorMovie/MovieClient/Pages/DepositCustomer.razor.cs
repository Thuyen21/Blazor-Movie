using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class DepositCustomer
{
#pragma warning disable CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string content;
#pragma warning restore CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private double cash = 0;
#pragma warning disable CS8618 // Non-nullable field 'clientToken' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string clientToken;
#pragma warning restore CS8618 // Non-nullable field 'clientToken' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string divCSS = "display: none;";
    private void DivCSS(string divCSS)
    {
        this.divCSS = divCSS;
    }

#pragma warning disable CS8618 // Non-nullable field 'objRef' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private DotNetObjectReference<DepositCustomer> objRef;
#pragma warning restore CS8618 // Non-nullable field 'objRef' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private bool doneCash = false;
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    private async Task Recharge()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        content = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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
