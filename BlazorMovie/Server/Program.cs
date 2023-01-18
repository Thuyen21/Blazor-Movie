using BlazorMovie.Server;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

/* It creates a `WebApplicationBuilder` object. */
var builder = WebApplication.CreateBuilder(args);

/* Setting the environment variable `GOOGLE_APPLICATION_CREDENTIALS` to the full path of the file
`movie2-e3c7b-firebase-adminsdk-dk3zo-cbfa735233.json`. */
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.GetFullPath(Path.Combine("movie2-e3c7b-firebase-adminsdk-dk3zo-cbfa735233.json")));
/* Creating a FirebaseApp object. */
FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromFile(Path.GetFullPath(Path.Combine("movie2-e3c7b-firebase-adminsdk-dk3zo-cbfa735233.json"))) });
/* Creating a FirestoreDb object. */
builder.Services.AddScoped(sp => FirestoreDb.Create("movie2-e3c7b"));
/* Creating a FirebaseAuthConfig object. */
builder.Services.AddScoped(sp => new FirebaseAuthConfig() { ApiKey = "AIzaSyAqCxl98i68Te5_xy3vgMcAEoF5qiBKE9o", 
    AuthDomain = "movie2-e3c7b.firebaseapp.com", Providers = new FirebaseAuthProvider[] { new EmailProvider() } });
/* Adding a singleton service to the service collection. */
builder.Services.AddSingleton<Censor>();
/* Adding a cookie authentication scheme to the service collection. */
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});
/* Increasing the size of the file that can be uploaded. */
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue; // <-- ! long.MaxValue
    options.MultipartBoundaryLengthLimit = int.MaxValue;
    options.MultipartHeadersCountLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

/* Adding a service to the service collection. */
builder.Services.AddControllersWithViews();
/* Adding a service to the service collection. */
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
/* A middleware that adds the AltSvc header to the response. */
app.Use((context, next) =>
{
    context.Response.Headers.AltSvc = "h3=\":443\"";
    return next(context);
});

/* It redirects HTTP requests to HTTPS. */
app.UseHttpsRedirection();

/* A middleware that serves static files from the Blazor framework. */
app.UseBlazorFrameworkFiles();
/* A middleware that serves static files from the wwwroot folder. */
app.UseStaticFiles();

/* A middleware that enables routing. */
app.UseRouting();

/* Adding the `AuthenticationMiddleware` to the request pipeline. */
app.UseAuthentication();
/* A middleware that enables authorization. */
app.UseAuthorization();

/* Mapping the Razor Pages. */
app.MapRazorPages();
/* It maps the controllers. */
app.MapControllers();
/* It maps the `index.html` file to the root path. */
app.MapFallbackToFile("index.html");

/* It starts the web server. */
app.Run();
