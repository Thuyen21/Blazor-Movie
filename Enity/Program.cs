using Enity.Data;
using BlazorMovie.Shared;
using Microsoft.OpenApi.Models;

try
{
    Console.WriteLine("Hello");
    Context context = new Context();
    AccountManagementModel accModel = new AccountManagementModel();
    accModel.Name = "haha123asdasdads";
    context.AccountManagementModels.Add(accModel);
    //context.SaveChanges();
    //accModel = context.AccountManagementModels.First(f => f.Id == accModel.Id);
    MovieModel movieModel = new MovieModel();
    movieModel.MovieName = "test";
    movieModel.Studio = accModel;
    movieModel.MovieGenre = "asd";
    context.Movies.Add(movieModel);
    
    context.SaveChanges();

}
catch (Exception ex)
{

    Console.WriteLine(ex.Message);
}



var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
