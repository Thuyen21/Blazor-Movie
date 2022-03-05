using Enity.Data;
using BlazorMovie.Shared;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

try
{
    // Console.WriteLine("Hello");
    Context ctx = new Context();
    //AccountManagementModel accountManagementModel = new AccountManagementModel();
    //accountManagementModel.Name = "accTest";
    MovieModel movieModel = new MovieModel();
    movieModel.MovieName = "Movie Test2";
    ctx.Movies.Add(movieModel);
    ////ctx.AccountManagementModels.Add(accountManagementModel);

    ////ctx.Movies.FirstOrDefault().Studio.Add(accountManagementModel);
    ctx.SaveChanges();

}
catch (Exception ex)
{

    Console.WriteLine(ex.InnerException.Message);
}



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("EnityContextConnection");builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<Context>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>();

builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
