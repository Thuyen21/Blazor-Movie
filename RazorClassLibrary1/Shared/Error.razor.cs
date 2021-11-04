using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace RazorClassLibrary1.Shared;

public partial class Error
{
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    public void ProcessError(Exception ex)
    {
        Logger.LogError("Error:ProcessError - Type: {Type} Message: {Message}", ex.GetType(), ex.Message);
    }
}
