using Microsoft.JSInterop;

namespace MovieClient.Shared;

public partial class NavMenu
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("NavMenu");
            }
        }
        catch (Exception ex)
        {

            
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
