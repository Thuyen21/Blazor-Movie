using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace RazorClassLibrary1.Pages;

public partial class StatusStudio
{
    [Parameter]
    public string Id { get; set; }

    private string content;
    private DateTime month = DateTime.UtcNow;
    private DateTime check = DateTime.UtcNow;
    private DateTime start = DateTime.UtcNow;
    private DateTime end = DateTime.UtcNow;
    private List<int>? commentStatus = new();
    private readonly List<Dictionary<string, string>>? fullStatus = new();
    private bool showAlert = false;
    private readonly Severity severity = Severity.Info;
    private void CloseAlert()
    {
        showAlert = false;
    }

    private async Task Salary()
    {
        content = await (await _httpClient.PostAsJsonAsync<List<string>>($"Studio/SalaryMovie", new List<string> { { Id }, { month.ToString("MM-dd-yyyy") } })).Content.ReadAsStringAsync();
        showAlert = true;
    }

    private async Task Check()
    {
        content = await (await _httpClient.PostAsJsonAsync<List<string>>($"Studio/Check", new List<string> { { Id }, { check.ToString("MM-dd-yyyy") } })).Content.ReadAsStringAsync();
        showAlert = true;
    }

    private async Task Submit()
    {
        fullStatus.Clear();
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
                List<double> getInfor = await _httpClient.GetFromJsonAsync<List<double>>($"Studio/PayCheck/{Id}/{date.ToString("MM-dd-yyyy")}");
                Dictionary<string, string> dic = new();
                dic.Add("Date", date.ToString());
                dic.Add("Positive", commentStatus[0].ToString());
                dic.Add("Negative", commentStatus[1].ToString());
                dic.Add("View", getInfor[0].ToString());
                dic.Add("Buy", getInfor[1].ToString());
                fullStatus.Add(dic);
                StateHasChanged();
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
