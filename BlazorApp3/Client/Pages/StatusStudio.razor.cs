using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlazorApp3.Client.Pages
{
	public partial class StatusStudio
	{
		[Parameter]
		public string Id { get; set; }

		protected Dictionary<string, dynamic> commentList = new();
		protected string view;
		protected string content;

		protected DateTime start = DateTime.UtcNow;
		protected DateTime end = DateTime.UtcNow;

		protected List<int> commentStatus = new();

		protected override async Task OnInitializedAsync()
		{
			//commentList = await _httpClient.GetFromJsonAsync<List<CommentModel>>($"Studio/Comment/{Id}");
			//commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}");
		}

		protected async Task Submit()
		{
			commentStatus.Clear();
			content = "Loading.....";
			//commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/check");
			commentStatus = await _httpClient.GetFromJsonAsync<List<int>>($"Studio/CommentStatus/{Id}/{start.ToString("MM dd yyyy")}/{end.ToString("MM dd yyyy")}");
			view = new string(await _httpClient.GetFromJsonAsync<char[]>($"Studio/View/{Id}/{start.ToString("MM dd yyyy")}/{end.ToString("MM dd yyyy")}"));
			content = "";
		}
	}
}