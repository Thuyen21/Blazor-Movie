using Microsoft.AspNetCore.Components;

namespace MovieClient.Shared;

public partial class SurveyPrompt
{
    // Demonstrates how a parent component can supply parameters
    [Parameter]
#pragma warning disable CS8618 // Non-nullable property 'Title' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Title { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Title' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
}
