using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class StatusStudio
{
    /* A parameter that is passed in from the parent component. */
    [Parameter]
    public string? Id { get; set; }
    /* A variable that is used to store the result of the API call. */
    private string? content;
    /* Setting the default value of the `month` variable to the current date. */
    private DateTime month = DateTime.UtcNow;
    /* Setting the default value of the `check` variable to the current date. */
    private DateTime check = DateTime.UtcNow;
    /* Setting the default value of the `start` variable to the current date. */
    private DateTime start = DateTime.UtcNow;
    /* Setting the default value of the `end` variable to the current date. */
    private DateTime end = DateTime.UtcNow;
    /* Creating a new list of integers. */
    private List<int>? commentStatus = new();
    /* Creating a new list of dictionaries. */
    private readonly ConcurrentBag<Dictionary<string, string>>? fullStatus = new();
    /* Setting the default value of the `showAlert` variable to `false`. */
    private bool showAlert = false;
    /* Setting the default value of the `severity` variable to `Severity.Info`. */
    private readonly Severity severity = Severity.Info;
    /// <summary>
    /// It closes the alert.
    /// </summary>
    private void CloseAlert()
    {
        showAlert = false;
    }

    /// <summary>
    /// It's a function that sends a request to the server and receives a response
    /// </summary>
    private async Task Salary()
    {
        content = await (await _httpClient.PostAsJsonAsync($"Studio/SalaryMovie", new List<string?> { { Id }, { month.ToString("MM-dd-yyyy") } })).Content.ReadAsStringAsync();
        showAlert = true;
    }

    /// <summary>
    /// It sends a post request to the server with the id of the studio and the date that the user wants
    /// to check
    /// </summary>
    private async Task Check()
    {
        content = await (await _httpClient.PostAsJsonAsync($"Studio/Check", new List<string?> { { Id }, { check.ToString("MM-dd-yyyy") } })).Content.ReadAsStringAsync();
        showAlert = true;
    }

    /// <summary>
    /// It's a function that gets data from the server and displays it in a table.
    /// 
    /// The problem is that the table is not updated until the function is finished.
    /// 
    /// I want to update the table as soon as the data is received.
    /// 
    /// I tried to use the InvokeAsync method, but it didn't work.
    /// 
    /// I also tried to use the StateHasChanged method, but it didn't work.
    /// 
    /// I also tried to use the IJSRuntime method, but it didn't work.
    /// 
    /// I also tried to use the IAsyncDisposable method, but it didn't work.
    /// 
    /// I also tried to use the IAsyncEnumerable method, but it didn't work.
    /// 
    /// I also tried to use the IAsyncEnumerator method, but it didn't work.
    /// 
    /// I also tried to use the I
    /// </summary>
    /// <returns>
    /// A Task.
    /// </returns>
    private Task Submit()
    {
        fullStatus!.Clear();
        content = "Loading.....";
        showAlert = true;
        try
        {
            List<DateTime> dateToCheck = new();
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                dateToCheck.Add(i);
            }

            _ = Parallel.ForEach(dateToCheck, async date =>
            {
                commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/{date:MM-dd-yyyy}");
                List<double> getInfor = await _httpClient.GetFromJsonAsync<List<double>>($"Studio/PayCheck/{Id}/{date:MM-dd-yyyy}") ?? new();
                Dictionary<string, string> dic = new()
                {
                    { "Date", date.ToString() },
                    { "Positive", commentStatus?[0].ToString() ?? string.Empty },
                    { "Negative", commentStatus?[1].ToString() ?? string.Empty},
                    { "View", getInfor[0].ToString() },
                    { "Buy", getInfor[1].ToString() }
                };
                fullStatus.Add(dic);

                await InvokeAsync(() => StateHasChanged());
            });
            content = "";
            showAlert = false;
        }
        catch (Exception ex)
        {
            content = ex.Message;
            showAlert = true;
        }

        return Task.CompletedTask;
    }
}
