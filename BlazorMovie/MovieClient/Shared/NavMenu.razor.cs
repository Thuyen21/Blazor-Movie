using Microsoft.JSInterop;

namespace MovieClient.Shared;

public partial class NavMenu
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("NavMenu");
        }
    }

    private bool collapseNavMenu = true;
#pragma warning disable CS8603 // Possible null reference return.
    private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;
#pragma warning restore CS8603 // Possible null reference return.
    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

    public void MenuChanged()
    {
        InvokeAsync(StateHasChanged);
    }
}
