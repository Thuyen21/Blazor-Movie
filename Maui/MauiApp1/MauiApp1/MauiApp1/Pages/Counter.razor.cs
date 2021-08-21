using BlazorApp3.Shared;
using Google.Cloud.Firestore;
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
    public partial class Counter
    {
        public int currentCount;
        protected AccountManagementModel acc = new();
        protected string mess = null;
        protected async Task IncrementCount()
        {
            currentCount++;
            try
            {
                FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
                Query usersRef = db.Collection("Account").WhereEqualTo("Id", "woD7khzuOvVmaRkPv2gnvSNMdOc2");
                QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    acc = document.ConvertTo<AccountManagementModel>();
                }
            }
            catch (Exception e)
            {
                mess = e.Message;
            }
        }
    }
}