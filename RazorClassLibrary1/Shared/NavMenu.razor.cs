using Microsoft.JSInterop;

namespace RazorClassLibrary1.Shared;

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
    private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;
    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

    public void MenuChanged()
    {
        InvokeAsync(StateHasChanged);
    }
}
