using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
    public partial class StatusStudio
    {
        [Parameter]
        public string Id { get; set; }



        protected string content;

        protected DateTime month = DateTime.UtcNow;
        protected DateTime check = DateTime.UtcNow;
        protected DateTime start = DateTime.UtcNow;
        protected DateTime end = DateTime.UtcNow;

        protected List<int>? commentStatus = new();
        private readonly List<Dictionary<string, string>>? fullStatus = new();
        protected override async Task OnInitializedAsync()
        {
            //commentList = await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Studio/Comment/{Id}");
            //commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}");
        }
        protected async Task Salary()
        {
            content = await (await _httpClient.PostAsJsonAsync<List<string>>($"Studio/SalaryMovie", new List<string> { { Id }, { month.ToString("MM dd yyyy") } })).Content.ReadAsStringAsync();

        }
        protected async Task Check()
        {
            content = await (await _httpClient.PostAsJsonAsync<List<string>>($"Studio/Check", new List<string> { { Id }, { check.ToString("MM dd yyyy") } })).Content.ReadAsStringAsync();

        }

        protected async Task Submit()
        {
            fullStatus.Clear();
            //commentStatus.Clear();
            content = "Loading.....";

            //for(DateTime i = start; i <= end; i = i.AddDays(1.0))
            //         {
            //	content = i.ToString();
            //}
            //commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/check");
            try
            {
                for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
                {
                    commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/{i.ToString("MM dd yyyy")}");


                    List<double> getInfor = await _httpClient.GetFromJsonAsync<List<double>>($"Studio/PayCheck/{Id}/{i.ToString("MM dd yyyy")}");
                    Dictionary<string, string> dic = new();
                    dic.Add("Date", i.ToString());
                    dic.Add("Positive", commentStatus[0].ToString());
                    dic.Add("Negative", commentStatus[1].ToString());
                    dic.Add("View", getInfor[0].ToString());
                    dic.Add("Buy", getInfor[1].ToString());

                    fullStatus.Add(dic);

                    content = "";
                }


            }
            catch (Exception ex)
            {
                content = ex.Message;
            }



        }
    }
}