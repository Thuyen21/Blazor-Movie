using BlazorApp3.Shared;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace BlazorApp3.Server.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IWebHostEnvironment env;
    private readonly FirestoreDb db;
    private readonly FirebaseAuthClient client;
    public AdminController(IWebHostEnvironment env, FirestoreDb db, FirebaseAuthClient client)
    {
        this.env = env;
        this.db = db;
        this.client = client;
    }

    [HttpGet("AccountManagement/{searchString?}/{sortOrder?}/{index:int:min(0)}")]
    public async Task<ActionResult<List<AccountManagementModel>>> AccountManagement(string? searchString, string? sortOrder, int index)
    {
        List<AccountManagementModel> myFoo = new();

        Query usersRef = db.Collection("Account");

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            usersRef = usersRef.OrderBy("Email").StartAt(searchString).EndAt(searchString + "~");
        }

        switch (sortOrder)
        {
            case "name":
                usersRef = usersRef.OrderBy("Name");
                break;
            case "nameDesc":
                usersRef = usersRef.OrderByDescending("Name");
                break;
            case "date":
                usersRef = usersRef.OrderBy("DateOfBirth");
                break;
            case "dateDesc":
                usersRef = usersRef.OrderByDescending("DateOfBirth");
                break;
            case "email":
                usersRef = usersRef.OrderBy("Email");
                break;
            case "emailDesc":
                usersRef = usersRef.OrderByDescending("Email");
                break;
        }
        usersRef = usersRef.Offset(index * 5).Limit(5);
        QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            AccountManagementModel convert = document.ConvertTo<AccountManagementModel>();


            myFoo.Add(convert);
        }
        return await Task.FromResult(myFoo);
    }
    [HttpGet("EditAccount/{Id}")]
    public async Task<ActionResult<AccountManagementModel>> EditAccount(string Id)
    {
        Query usersRef = db.Collection("Account").WhereEqualTo("Id", Id);

        QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

        AccountManagementModel account = new();

        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            account = snapshotDocument.ConvertTo<AccountManagementModel>();
        }


        return await Task.FromResult(account);
    }
    [HttpPost("EditAccount")]
    public async Task<ActionResult<AccountManagementModel>> EditAccount([FromBody] AccountManagementModel account)
    {
        if (account.Role == "Admin" || account.Role == "Studio" || account.Role == "Customer" ||
            account.Role == "Admin")
        {
        }

        else

        {

            return BadRequest("Don't edit role");
        }

        try
        {
            Query collection = db.Collection("Account").WhereEqualTo("Id", account.Id);

            QuerySnapshot snapshot = await collection.GetSnapshotAsync();


            Dictionary<string, dynamic> update = new()
            {
                { "Name", account.Name },
                { "Role", account.Role },
                { "DateOfBirth", account.DateOfBirth.ToUniversalTime() }
            };

            foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
            {
                await snapshotDocument.Reference.UpdateAsync(update);
            }


            return Ok("Success");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }


    }
    [HttpPost("Ban")]
    public async Task<ActionResult> Ban([FromBody] string Id)
    {
        try
        {

            string path = Path.GetFullPath(Path.Combine("movie2-e3c7b-firebase-adminsdk-dk3zo-cbfa735233.json"));

            FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromFile(path) });

            UserRecordArgs userRecordArgs = new UserRecordArgs() { Uid = Id, Disabled = true };
            await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);

            FirebaseApp.DefaultInstance.Delete();
            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("UnBan")]
    public async Task<ActionResult> UnBan([FromBody] string Id)
    {
        try
        {

            string path = Path.GetFullPath(Path.Combine("movie2-e3c7b-firebase-adminsdk-dk3zo-cbfa735233.json"));

            FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromFile(path) });

            UserRecordArgs userRecordArgs = new UserRecordArgs() { Uid = Id, Disabled = false };
            await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);

            FirebaseApp.DefaultInstance.Delete();
            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("Movie/{searchString?}/{sortOrder?}/{index:int:min(0)}")]
    public async Task<ActionResult<List<MovieModel>>> Movie(string? sortOrder, string? searchString, int index)
    {
        

        Query usersRef = db.Collection("Movie");
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            usersRef = usersRef.OrderBy("MovieName").StartAt(searchString).EndAt(searchString + "~");
        }

        switch (sortOrder)
        {
            case "name":
                usersRef = usersRef.OrderBy("MovieName");
                break;
            case "nameDesc":
                usersRef = usersRef.OrderByDescending("MovieName");
                break;
            case "date":
                usersRef = usersRef.OrderBy("PremiereDate");
                break;
            case "dateDesc":
                usersRef = usersRef.OrderByDescending("PremiereDate");
                break;
            case "genre":
                usersRef = usersRef.OrderBy("MovieGenre");
                break;
            case "genreDesc":
                usersRef = usersRef.OrderByDescending("MovieGenre");
                break;
        }

        List<MovieModel> myFoo = new();
        usersRef = usersRef.Offset(index * 5).Limit(5);
        QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();


        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            myFoo.Add(document.ConvertTo<MovieModel>());
        }

        return await Task.FromResult(myFoo);
    }
    [HttpGet("EditMovie/{Id}")]
    public async Task<ActionResult<MovieModel>> EditMovie(string Id)
    {
        
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", Id);

        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        MovieModel movie = new();

        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            movie = snapshotDocument.ConvertTo<MovieModel>();
        }

        return await Task.FromResult(movie);
    }
    [HttpPost("EditMovie")]
    public async Task<ActionResult> EditMoviePost([FromBody] MovieModel movie)
    {
        
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", movie.MovieId);
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();

        movie.PremiereDate = movie.PremiereDate.ToUniversalTime();

#pragma warning disable CS8619 // Nullability of reference types in value of type 'Dictionary<string, object?>' doesn't match target type 'Dictionary<string, dynamic>'.
        Dictionary<string, dynamic> dictionary = movie.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(movie, null));
#pragma warning restore CS8619 // Nullability of reference types in value of type 'Dictionary<string, object?>' doesn't match target type 'Dictionary<string, dynamic>'.

        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            await snapshotDocument.Reference.UpdateAsync(dictionary);
        }
        return Ok("Success");
    }

    [HttpGet("MovieUpload/{MovieId}")]
    public async Task<ActionResult> MovieUpload(string MovieId)
    {
        
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", MovieId);

        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        MovieModel movie = new();

        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            movie = snapshotDocument.ConvertTo<MovieModel>();
        }

        return View(movie);
    }
    [HttpGet("Done")]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    public async Task<ActionResult> Done()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
    {
        return View();
    }
    [HttpPost("MovieUpload/{MovieId}/{StudioId}")]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<ActionResult> MovieUpload(string StudioId, string MovieId, IFormFile ImageFileUp, IFormFile MovieFileUp)
    {
        if (ImageFileUp == null)
        {

        }
        else
        {
            List<string> list = new List<string>
                {
                    "image/bmp",
                    "image/gif",
                    "image/jpeg",
                    "image/png",
                    "image/svg+xml",
                    "image/tiff",
                    "image/webp"
                };
            if (list.Contains(ImageFileUp.ContentType))
            {
                using Stream fileStream = ImageFileUp.OpenReadStream();
                {


                    FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com",
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token")),
                                ThrowOnCancel = true,
                                HttpClientTimeout = TimeSpan.FromHours(2)
                            }).Child(StudioId).Child(MovieId).Child("Image")
                        .PutAsync(fileStream);


                    task.Progress.ProgressChanged += (s, e) =>
                    {

                    };

                    await task;

                    fileStream.Close();
                }
            }

        }
        if (MovieFileUp == null)
        {

        }
        else
        {
            List<string> list = new List<string>
                {
                    "video/x-msvideo",
                    "video/mp4",
                    "video/mpeg",
                    "video/ogg",
                    "video/mp2t",
                    "video/webm",
                    "video/3gpp",
                    "video/3gpp2",
                    "video/x-matroska"
                };

            if (list.Contains(MovieFileUp.ContentType))
            {
                using Stream fileStream = MovieFileUp.OpenReadStream();
                {
                    FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com",
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token")),
                                ThrowOnCancel = true,
                                HttpClientTimeout = TimeSpan.FromHours(2)
                            }).Child(StudioId).Child(MovieId).Child("Movie")
                        .PutAsync(fileStream);


                    task.Progress.ProgressChanged += (s, e) =>
                    {

                    };

                    await task;

                    fileStream.Close();
                }
            }

        }


        string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        string redirect = hostname + "/Admin/Done";
        return Redirect(redirect);
    }
    [HttpPost("DeleteMovie")]

    public async Task<ActionResult> DeleteMovie([FromBody] MovieModel movie)
    {
        
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", movie.MovieId);

        QuerySnapshot snapshot = await collection.GetSnapshotAsync();

        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            movie = snapshotDocument.ConvertTo<MovieModel>();


            Task delete = new FirebaseStorage("movie2-e3c7b.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token"))
                    }).Child(movie.StudioId).Child(movie.MovieId).Child("Image")
                .DeleteAsync();
            await delete;
            delete = new FirebaseStorage("movie2-e3c7b.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token"))
                    }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie")
                .DeleteAsync();
            await delete;
            await snapshotDocument.Reference.DeleteAsync();
            return Ok("Success");
        }
        return BadRequest("Not success");
    }
}
