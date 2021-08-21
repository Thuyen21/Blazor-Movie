using BlazorApp3.Shared;
using System.Net.Http.Json;
using BlazorApp3.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using MauiApp1;
using MauiApp1.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using MauiApp1.Services;
namespace MauiApp1.Pages
{
    public partial class MovieStudio
    {
        protected List<MovieModel> movies = new();
        protected int index = 0;
        protected string? searchString { get; set; }
        protected bool isSearch = false;

        protected string sort = null;

        protected async Task NameSortParm()
        {
            index = 0;
            sort = sort == "name" ? "nameDesc" : "name";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        protected async Task DateSortParm()
        {
            index = 0;
            sort = sort == "date" ? "dateDesc" : "date";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        protected async Task GenreSortParm()
        {
            index = 0;
            sort = sort == "genre" ? "genreDesc" : "genre";
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}");
            isSearch = false;
            searchString = null;
        }

        protected override async Task OnInitializedAsync()
        {
            movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}");
        }
        protected async Task Search()
        {
            index = 0;
            if (searchString != null)
            {
                movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/{searchString}/ /{index}");
                isSearch = true;
            }
            else
            {
                movies = await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}");
                isSearch = false;
            }


            sort = null;
        }
        protected async Task LoadMore()
        {
            index++;
            if (isSearch)
            {
                movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/{searchString}//{index}"));
            }
            else if (sort != null)
            {
                movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ /{sort}/{index}"));
            }
            else
            {
                movies.AddRange(await _httpClient.GetFromJsonAsync<List<MovieModel>>($"Studio/Index/ / /{index}"));
            }

        }
    }
}