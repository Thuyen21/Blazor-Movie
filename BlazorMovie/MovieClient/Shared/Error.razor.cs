using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MovieClient.Shared;

public partial class Error
{
    [Parameter]
#pragma warning disable CS8618 // Non-nullable property 'ChildContent' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public RenderFragment ChildContent { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ChildContent' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    public void ProcessError(Exception ex)
    {
        Logger.LogError("Error:ProcessError - Type: {Type} Message: {Message}", ex.GetType(), ex.Message);
    }
}
