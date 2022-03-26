using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class StatusStudio
{
    [Parameter]
    public string? Id { get; set; }
    private string? content;
    private DateTime month = DateTime.UtcNow;
    private DateTime check = DateTime.UtcNow;
    private DateTime start = DateTime.UtcNow;
    private DateTime end = DateTime.UtcNow;
    private List<int>? commentStatus = new();
    private readonly ConcurrentBag<Dictionary<string, string>>? fullStatus = new();
    private bool showAlert = false;
    private readonly Severity severity = Severity.Info;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task Salary()
    {
        content = await (await _httpClient.PostAsJsonAsync($"Studio/SalaryMovie", new List<string> { { Id }, { month.ToString("MM-dd-yyyy") } })).Content.ReadAsStringAsync();
        showAlert = true;
    }

    private async Task Check()
    {
        content = await (await _httpClient.PostAsJsonAsync($"Studio/Check", new List<string> { { Id }, { check.ToString("MM-dd-yyyy") } })).Content.ReadAsStringAsync();
        showAlert = true;
    }

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
                List<double> getInfor = await _httpClient.GetFromJsonAsync<List<double>>($"Studio/PayCheck/{Id}/{date:MM-dd-yyyy}");
                Dictionary<string, string> dic = new()
                {
                    { "Date", date.ToString() },
                    { "Positive", commentStatus[0].ToString() },
                    { "Negative", commentStatus[1].ToString() },
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
