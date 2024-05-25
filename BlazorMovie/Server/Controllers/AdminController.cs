/* The above code is a controller that is used to manage the admin page. */
using BlazorMovie.Shared;
using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace BlazorMovie.Server.Controllers;
[ApiController]
/* A route attribute that tells the controller to use the name of the controller as the route. */
[Route("[controller]")]
/* Restricting access to the controller to only users who are in the Admin role. */
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    /* Creating a new instance of the FirestoreDb class. */
    private readonly FirestoreDb db;
    /* The above code is creating a new instance of the FirebaseAuthClient class. */
    public AdminController(IWebHostEnvironment env, FirestoreDb db, FirebaseAuthConfig config)
    {
        this.db = db;
    }

    /// <summary>
    /// I'm trying to get a list of users from the database, and then return them to the front end
    /// </summary>
    /// <param name="searchString">The search string that the user entered in the search box.</param>
    /// <param name="sortOrder">This is the name of the column that the user clicked on to sort the
    /// data.</param>
    /// <param name="index">The page number</param>
    /// <returns>
    /// A list of AccountManagementModel objects.
    /// </returns>
    [HttpGet("AccountManagement/{searchString?}/{sortOrder?}/{index:int:min(0)}")]
    public async Task<ActionResult<List<AccountManagementModel>>> AccountManagement(string? searchString, string? sortOrder, int index)
    {
        try
        {
            List<AccountManagementModel> myFoo = [];

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
            return await Task.FromResult(Ok(myFoo));
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }

    }
    /// <summary>
    /// It gets the account information from the database and returns it to the user.
    /// </summary>
    /// <param name="Id">The Id of the account to be edited.</param>
    /// <returns>
    /// The account information for the user with the Id that was passed in.
    /// </returns>
    [HttpGet("EditAccount/{Id}")]
    public async Task<ActionResult<AccountManagementModel>> EditAccount(string Id)
    {
        try
        {
            Query usersRef = db.Collection("Account").WhereEqualTo("Id", Id);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            AccountManagementModel account = new();
            foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
            {
                account = snapshotDocument.ConvertTo<AccountManagementModel>();
            }
            return await Task.FromResult(Ok(account));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// A function to edit the account information.
    /// </summary>
    /// <param name="AccountManagementModel">This is a model that contains the data that will be
    /// updated.</param>
    /// <returns>
    /// The return type is an ActionResult<AccountManagementModel>
    /// </returns>
    [HttpPost("EditAccount")]
    public async Task<ActionResult<AccountManagementModel>> EditAccount([FromBody] AccountManagementModel account)
    {
        try
        {
            if (account.Role is "Admin" or "Studio" or "Customer" or
            "Admin")
            {

                Query collection = db.Collection("Account").WhereEqualTo("Id", account.Id);
                QuerySnapshot snapshot = await collection.GetSnapshotAsync();
                Dictionary<string, dynamic?> update = new()
            {
                { "Name", account.Name },
                { "Role", account.Role },
                { "DateOfBirth", account.DateOfBirth.ToUniversalTime() }
            };
                _ = Parallel.ForEach(snapshot.Documents, async snapshotDocument =>
                {
                    _ = await snapshotDocument.Reference.UpdateAsync(update);
                });
                return Ok("Success");
            }
            else
            {
                return BadRequest("Don't edit role");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// It takes a user's ID, and then bans them
    /// </summary>
    /// <param name="Id">The user's ID.</param>
    /// <returns>
    /// The user's ID is being returned.
    /// </returns>
    [HttpPost("Ban")]
    public async Task<ActionResult> Ban([FromBody] string Id)
    {
        try
        {
            UserRecordArgs userRecordArgs = new() { Uid = Id, Disabled = true };
            _ = await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);
            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// It takes a user's ID, and then un-bans them
    /// </summary>
    /// <param name="Id">The user's ID.</param>
    /// <returns>
    /// The user's ID is being returned.
    /// </returns>
    [HttpPost("UnBan")]
    public async Task<ActionResult> UnBan([FromBody] string Id)
    {
        try
        {
            UserRecordArgs userRecordArgs = new() { Uid = Id, Disabled = false };
            _ = await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);
            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// I'm trying to get a list of movies from a database, and I'm trying to sort them by name, genre,
    /// or date.
    /// </summary>
    /// <param name="sortOrder">The sort order of the list.</param>
    /// <param name="searchString">The string that the user is searching for.</param>
    /// <param name="index">The index of the page to return.</param>
    /// <returns>
    /// A list of movies.
    /// </returns>
    [HttpGet("Movie/{searchString?}/{sortOrder?}/{index:int:min(0)}")]
    public async Task<ActionResult<List<MovieModel>>> Movie(string? sortOrder, string? searchString, int index)
    {
        try
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
            List<MovieModel> myFoo = [];
            usersRef = usersRef.Offset(index * 5).Limit(5);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                myFoo.Add(document.ConvertTo<MovieModel>());
            }
            return await Task.FromResult(Ok(myFoo));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    /// <summary>
    /// It gets the movie from the database and returns it to the client.
    /// </summary>
    /// <param name="Id">The Id of the movie you want to edit.</param>
    /// <returns>
    /// The movie with the Id that was passed in.
    /// </returns>
    [HttpGet("EditMovie/{Id}")]
    public async Task<ActionResult<MovieModel>> EditMovie(string Id)
    {
        try
        {
            Query collection = db.Collection("Movie").WhereEqualTo("MovieId", Id);
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();
            MovieModel movie = new();
            foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
            {
                movie = snapshotDocument.ConvertTo<MovieModel>();
            }
            return await Task.FromResult(Ok(movie));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// It takes a movie object, gets the movie from the database, updates the movie in the database
    /// with the movie object, and returns a success message
    /// </summary>
    /// <param name="MovieModel"></param>
    /// <returns>
    /// The method returns an HTTP 200 OK response with the string "Success" in the body.
    /// </returns>
    [HttpPost("EditMovie")]
    public async Task<ActionResult> EditMoviePost([FromBody] MovieModel movie)
    {
        try
        {
            Query collection = db.Collection("Movie").WhereEqualTo("MovieId", movie.MovieId);
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();
            movie.PremiereDate = movie.PremiereDate.ToUniversalTime();
            Dictionary<string, dynamic?> dictionary = movie.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(movie, null));

            _ = Parallel.ForEach(snapshot.Documents, async snapshotDocument =>
            {
                _ = await snapshotDocument.Reference.UpdateAsync(dictionary);
            });
            return Ok("Success");
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// This function is used to get the movie details from the database and display it on the view
    /// </summary>
    /// <param name="MovieId">The unique identifier for the movie.</param>
    /// <returns>
    /// The MovieUpload view is being returned.
    /// </returns>
    [HttpGet("MovieUpload/{MovieId}")]
    public async Task<ActionResult> MovieIdUpload(string MovieId)
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
    /// <summary>
    /// It returns a view called "Done" when the user navigates to the URL /Done
    /// </summary>
    /// <returns>
    /// A view
    /// </returns>
    [HttpGet("Done")]
    public ActionResult Done()
    {
        return View();
    }
    /// <summary>
    /// It uploads a movie and an image to firebase storage
    /// </summary>
    /// <param name="StudioId">The ID of the studio that the movie belongs to.</param>
    /// <param name="MovieId">The ID of the movie that is being uploaded.</param>
    /// <param name="IFormFile">The file that is being uploaded.</param>
    /// <param name="IFormFile">The file that is being uploaded.</param>
    /// <returns>
    /// The code is returning a redirect to the hostname.
    /// </returns>
    [HttpPost("MovieUpload/{MovieId}/{StudioId}")]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<ActionResult> MovieUpload(string StudioId, string MovieId, IFormFile ImageFileUp, IFormFile MovieFileUp)
    {
        if (ImageFileUp != null)
        {
            List<string> list =
            [
                "image/bmp",
                "image/gif",
                "image/jpeg",
                "image/png",
                "image/svg+xml",
                "image/tiff",
                "image/webp"
            ];
            if (list.Contains(ImageFileUp.ContentType))
            {
                using Stream fileStream = ImageFileUp.OpenReadStream();

                FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token")),
                            ThrowOnCancel = true,
                            HttpClientTimeout = TimeSpan.FromHours(2)
                        }).Child(StudioId).Child(MovieId).Child("Image")
                    .PutAsync(fileStream);
                _ = await task;

                fileStream.Close();

            }
        }
        if (MovieFileUp != null)
        {
            List<string> list =
            [
                "video/x-msvideo",
                "video/mp4",
                "video/mpeg",
                "video/ogg",
                "video/mp2t",
                "video/webm",
                "video/3gpp",
                "video/3gpp2",
                "video/x-matroska"
            ];
            if (list.Contains(MovieFileUp.ContentType))
            {
                using Stream fileStream = MovieFileUp.OpenReadStream();

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

                _ = await task;

                fileStream.Close();

            }

        }
        string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        string redirect = hostname;
        return Redirect(redirect);
    }

    /// <summary>
    /// It deletes the movie from the database and deletes the movie and image from the storage
    /// </summary>
    /// <param name="MovieModel">This is the model that I'm using to store the data in the
    /// database.</param>
    /// <returns>
    /// The movie is being deleted from the database and the storage.
    /// </returns>
    [HttpPost("DeleteMovie")]
    public async Task<ActionResult> DeleteMovie([FromBody] MovieModel movie)
    {
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", movie.MovieId);
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            movie = snapshotDocument.ConvertTo<MovieModel>();
            try
            {
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
            }
            catch
            {
                return BadRequest("Not success");
            }
        }
        return Ok("Success");
    }
    /// <summary>
    /// It takes an index, and returns the next 5 feedback messages from the database, ordered by time,
    /// starting from the index
    /// </summary>
    /// <param name="index">The index of the page you want to get.</param>
    /// <returns>
    /// A list of FeedbackMessageModel objects.
    /// </returns>
    [HttpGet("ReadFeedBack/{index:int:min(0)}")]
    public async Task<ActionResult<List<FeedbackMessageModel>>> ReadFeedBack(int index)
    {
        index--;
        List<FeedbackMessageModel> myFoo = [];
        Query usersRef = db.Collection("Feedback");
        usersRef = usersRef.OrderByDescending("Time").Offset(index * 5).Limit(5);
        QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            FeedbackMessageModel convert = document.ConvertTo<FeedbackMessageModel>();
            myFoo.Add(convert);
        }
        return await Task.FromResult(Ok(myFoo));
    }
}
