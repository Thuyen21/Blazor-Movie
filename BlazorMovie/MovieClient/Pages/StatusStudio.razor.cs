using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace MovieClient.Pages;

public partial class StatusStudio
{
    [Parameter]
#pragma warning disable CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    private string content;
#pragma warning restore CS8618 // Non-nullable field 'content' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
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

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    private async Task Submit()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        fullStatus.Clear();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        //commentStatus.Clear();
        content = "Loading.....";
        showAlert = true;
        //for(DateTime i = start; i <= end; i = i.AddDays(1.0))
        // {
        // content = i.ToString();
        //}
        //commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/check");
        try
        {
            List<DateTime> dateToCheck = new();
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                dateToCheck.Add(i);
            }

            Parallel.ForEach(dateToCheck, async date =>
            {
                commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/{date.ToString("MM-dd-yyyy")}");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                List<double> getInfor = await _httpClient.GetFromJsonAsync<List<double>>($"Studio/PayCheck/{Id}/{date.ToString("MM-dd-yyyy")}");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                Dictionary<string, string> dic = new();
                dic.Add("Date", date.ToString());
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                dic.Add("Positive", commentStatus[0].ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                dic.Add("Negative", commentStatus[1].ToString());
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                dic.Add("View", getInfor[0].ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                dic.Add("Buy", getInfor[1].ToString());
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
    }
}
