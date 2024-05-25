using BlazorMovie.Shared;
using BlazorMovie_Server;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayoutsSdk.Core;
using PayoutsSdk.Payouts;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;

namespace BlazorMovie.Server.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Studio")]
public class StudioController : Controller
{
    private readonly FirestoreDb db;
    public StudioController(FirestoreDb db)
    {
        this.db = db;
    }
    /// <summary>
    /// It takes in a search string, a sort order, and an index, and returns a list of movies that match
    /// the search string, sorted by the sort order, and offset by the index.
    /// </summary>
    /// <param name="searchString">The search string that the user entered in the search box.</param>
    /// <param name="sortOrder">The sort order of the movies.</param>
    /// <param name="index">the page number</param>
    /// <returns>
    /// A list of movies.
    /// </returns>
    [HttpGet("Index/{searchString?}/{sortOrder?}/{index:int:min(0)}")]
    public async Task<ActionResult<List<MovieModel>>> Index(string? searchString, string? sortOrder, int index)
    {
        try
        {
            Query usersRef = db.Collection("Movie").WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid));
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
        catch
        {
            return BadRequest();
        }

    }
    /// <summary>
    /// It takes a movie object, adds it to the database, and returns the id of the movie
    /// </summary>
    /// <param name="MovieModel">This is the model that I'm using to store the data.</param>
    /// <returns>
    /// The MovieId is being returned.
    /// </returns>
    [HttpPost("Upload")]
    public async Task<ActionResult> Upload([FromBody] MovieModel movie)
    {
        CollectionReference collection = db.Collection("Movie");
        try
        {
            movie.PremiereDate = movie.PremiereDate.ToUniversalTime();
            movie.StudioId = User.FindFirstValue(ClaimTypes.Sid);
            DocumentReference MovieId = await collection.AddAsync(movie);
            _ = await MovieId.UpdateAsync(new Dictionary<string, dynamic> { { "MovieId", MovieId.Id } });
            return Ok("You can go to edit this movie to upload movie and image");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    /// <summary>
    /// It gets the movie from the database and returns it to the user.
    /// </summary>
    /// <param name="Id">The Id of the movie to edit</param>
    /// <returns>
    /// A MovieModel object.
    /// </returns>
    [HttpGet("EditMovie/{Id}")]
    public async Task<ActionResult<MovieModel>> EditMovie(string Id)
    {
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", Id).WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid));
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        MovieModel movie = new();

        if (snapshot.Documents.Count < 1)
        {
            return new MovieModel();
        }
        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            movie = snapshotDocument.ConvertTo<MovieModel>();
        }

        return await Task.FromResult(movie);
    }
    /// <summary>
    /// It takes a movie object, finds all documents in the Movie collection that match the movieId, and
    /// updates all of those documents with the new movie object
    /// </summary>
    /// <param name="MovieModel">This is the model that I'm using to pass the data to the API.</param>
    /// <returns>
    /// The method returns an HTTP 200 OK response with the string "Success" in the body.
    /// </returns>
    [HttpPost("EditMovie")]
    public async Task<ActionResult> EditMovie([FromBody] MovieModel movie)
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
    /// <summary>
    /// The function returns a view called "Done"
    /// </summary>
    /// <returns>
    /// A view called Done.
    /// </returns>
    [HttpGet("Done")]
    public async Task<ActionResult> Done()
    {
        return await Task.FromResult(View());
    }
    /// <summary>
    /// It gets the movie from the database and returns it to the view.
    /// </summary>
    /// <param name="MovieId">The MovieId is the unique identifier for the movie.</param>
    /// <returns>
    /// A view of the movie model.
    /// </returns>
    [HttpGet("MovieUpload/{MovieId}")]
    public async Task<ActionResult> MovieUpload(string MovieId)
    {
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", MovieId).WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid));
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        MovieModel movie = new();
        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            movie = snapshotDocument.ConvertTo<MovieModel>();
        }
        return View(movie);
    }
    /// <summary>
    /// It takes a movie id and a studio id, and uploads an image and a movie to firebase storage.
    /// </summary>
    /// <param name="StudioId">The id of the studio that the movie belongs to.</param>
    /// <param name="movieId">The ID of the movie that is being uploaded.</param>
    /// <param name="IFormFile">The file that is being uploaded.</param>
    /// <param name="IFormFile">The file that is being uploaded.</param>
    /// <returns>
    /// The code is returning a redirect to the hostname.
    /// </returns>
    [HttpPost("MovieUpload/{MovieId}")]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<ActionResult> MovieUpload(string movieId, IFormFile imageFileUp, IFormFile movieFileUp)
    {
        string? studioId = User.FindFirstValue(ClaimTypes.Sid);
        if (imageFileUp != null)
        {
            List<string> list = new()
            {
                "image/bmp",
                "image/gif",
                "image/jpeg",
                "image/png",
                "image/svg+xml",
                "image/tiff",
                "image/webp"
            };
            if (list.Contains(imageFileUp.ContentType))
            {
                using Stream fileStream = imageFileUp.OpenReadStream();



                FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token")),
                            ThrowOnCancel = true,
                            HttpClientTimeout = TimeSpan.FromHours(2)
                        }).Child(studioId).Child(movieId).Child("Image")
                    .PutAsync(fileStream);


                task.Progress.ProgressChanged += (s, e) =>
                {

                };

                _ = await task;

                fileStream.Close();
            }

        }
        if (movieFileUp != null)
        {
            List<string> list = new()
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

            if (list.Contains(movieFileUp.ContentType))
            {
                using Stream fileStream = movieFileUp.OpenReadStream();

                FirebaseStorageTask task = new FirebaseStorage("movie2-e3c7b.appspot.com",
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token")),
                            ThrowOnCancel = true,
                            HttpClientTimeout = TimeSpan.FromHours(2)
                        }).Child(studioId).Child(movieId).Child("Movie")
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
    /// It's a function that gets the comments of a movie from the database
    /// </summary>
    /// <param name="Id">The id of the movie</param>
    /// <returns>
    /// A list of comments.
    /// </returns>
    [HttpGet("Comment/{Id}")]
    public async Task<ActionResult<List<CommentModel>>> Comment(string Id)
    {
        Query commentSend = db.Collection("Comment").WhereEqualTo("MovieId", Id).OrderByDescending("Time");
        QuerySnapshot commentSnapshot = await commentSend.GetSnapshotAsync();
        List<CommentModel> commentList = new();

        foreach (DocumentSnapshot item in commentSnapshot.Documents)
        {
            CommentModel commentConvert = item.ConvertTo<CommentModel>();
            //int like = (await db.Collection("CommentAcction").WhereEqualTo("CommentId", item.Id).WhereEqualTo("Action", "Like").GetSnapshotAsync()).Documents.Count;
            //int Dislike = (await db.Collection("CommentAcction").WhereEqualTo("CommentId", item.Id).WhereEqualTo("Action", "DisLike").GetSnapshotAsync()).Documents.Count;
            //commentList.Add(new CommentModel() { Id = commentConvert.Id, Email = commentConvert.Email, MovieId = commentConvert.MovieId, Time = commentConvert.Time, CommentText = commentConvert.CommentText, Like = like, DisLike = Dislike });
            commentList.Add(commentConvert);
        }

        return await Task.FromResult(commentList);
    }
    //[HttpGet("View/{Id}/{Start}")]
    //public async Task<ActionResult<char[]>> View(string Id, string Start)
    //{
    //	try
    //	{
    //		DateTime StartDate = DateTime.Parse(Start).AddHours(12);
    //		DateTime EndDate = StartDate.AddDays(1);
    //		

    //		Query view = db.Collection("View").WhereEqualTo("Id", Id).OrderByDescending("Time").WhereGreaterThanOrEqualTo("Time", StartDate.ToUniversalTime()).WhereLessThanOrEqualTo("Time", EndDate.ToUniversalTime());
    //		QuerySnapshot viewSnapshot = await view.GetSnapshotAsync();

    //		return viewSnapshot.Documents.Count.ToString().ToCharArray();
    //	}
    //	catch (Exception)
    //	{
    //		return "0".ToCharArray();
    //	}

    //}

    /// <summary>
    /// It returns the number of views and purchases of a movie in a given day
    /// </summary>
    /// <param name="Id">Movie ID</param>
    /// <param name="Start">The start time of the day, in the format of "yyyy-MM-dd"</param>
    /// <returns>
    /// A list of doubles
    /// </returns>
    [HttpGet("PayCheck/{Id}/{Start}")]
    public async Task<ActionResult<List<double>>> PayCheck(string id, string start)
    {
        DateTime startDate = DateTime.Parse(start, new CultureInfo("en-US")).AddHours(12).ToUniversalTime();

        DateTime endDate = startDate.AddDays(1);
        try
        {
            double viewCount = (await db.Collection("View").WhereGreaterThanOrEqualTo("Time", startDate).WhereLessThanOrEqualTo("Time", endDate).GetSnapshotAsync()).Documents.Count;
            if (viewCount is 0)
            {
                double buy0 = (await db.Collection("Buy").WhereEqualTo("MovieId", id).WhereGreaterThanOrEqualTo("Time", startDate).WhereLessThanOrEqualTo("Time", endDate).GetSnapshotAsync()).Documents.Count;
                //double m0 = buy0 * 4.49;
                List<double> result0 = new()
                {
                    0,
                    buy0
                };
                return result0;
            }
            double viewt = (await db.Collection("View").WhereEqualTo("Id", id).WhereGreaterThanOrEqualTo("Time", startDate).WhereLessThanOrEqualTo("Time", endDate).GetSnapshotAsync()).Documents.Count;
            double buy = (await db.Collection("Buy").WhereEqualTo("MovieId", id).WhereGreaterThanOrEqualTo("Time", startDate).WhereLessThanOrEqualTo("Time", endDate).GetSnapshotAsync()).Documents.Count;
            //double vip = (await db.Collection("Vip").WhereGreaterThanOrEqualTo("Time", startDate).GetSnapshotAsync()).Documents.Count;

            List<double> result = new()
            {
                viewt,
                buy
            };


            return result;
        }
        catch (Exception)
        {
            return BadRequest();
        }

    }
    /// <summary>
    /// This function is used to get the number of positive and negative comments for a particular movie
    /// </summary>
    /// <param name="Id">MovieId</param>
    /// <param name="Start">The start date of the movie</param>
    /// <returns>
    /// A list of integers.
    /// </returns>
    [HttpGet("CommentStatus/{Id}/{Start}")]
    public async Task<ActionResult<List<int>>> CommentStatus(string Id, string Start)
    {
        try
        {
            DateTime StartDate = DateTime.Parse(Start, new CultureInfo("en-US")).AddHours(12);
            DateTime EndDate = StartDate.AddDays(1);


            Query commentSend = db.Collection("Comment").WhereEqualTo("MovieId", Id).OrderByDescending("Time").WhereGreaterThanOrEqualTo("Time", StartDate.ToUniversalTime()).WhereLessThanOrEqualTo("Time", EndDate.ToUniversalTime());
            QuerySnapshot commentSnapshot = await commentSend.GetSnapshotAsync();

            List<int> list = new()
            {
                0,
                0
            };

            MLModel.ModelInput sampleData = new();
            foreach (DocumentSnapshot item in commentSnapshot.Documents)
            {
                CommentModel commentConvert = item.ConvertTo<CommentModel>();

                if (commentConvert.Prediction == null)
                {
                    //Load sample data
                    sampleData.Review = commentConvert.CommentText;
                    //Load model and predict output
                    MLModel.ModelOutput result = MLModel.Predict(sampleData);
                    if (result.PredictedLabel == "positive")
                    {
                        list[0] = list[0] + 1;
                        _ = await item.Reference.UpdateAsync(new Dictionary<string, dynamic> { { "Prediction", "Positive" } });
                    }
                    else
                    {
                        list[1] = list[1] + 1;
                        _ = await item.Reference.UpdateAsync(new Dictionary<string, dynamic> { { "Prediction", "Negative" } });
                    }
                }
                else
                {
                    if (commentConvert.Prediction == "Positive")
                    {
                        list[0] = list[0] + 1;
                    }
                    else
                    {
                        list[1] = list[1] + 1;
                    }
                }

            }

            return list;
        }
        catch (Exception)
        {
            return Ok();
        }

    }
    /// <summary>
    /// I want to get the value of the field in the document and then update the value of the field in the
    /// document
    /// </summary>
    /// <param name="ss">List<string></param>
    [HttpPost("SalaryMovie")]
    public async Task<ActionResult> SalaryMovie([FromBody] List<string> ss)
    {

        if (DateTime.Parse(ss[1], new CultureInfo("en-US")).Month > DateTime.UtcNow.Month - 1)
        {
            return BadRequest("Cant Salary In lower than month now -1");
        }
        try
        {
            ss[1] = DateTime.Parse(ss[1], new CultureInfo("en-US")).ToString("MM yyyy");

            QuerySnapshot? snapshot = await db.Collection("Movie").WhereEqualTo("MovieId", ss[0]).WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync();
            double cash = snapshot.Documents[0].GetValue<double>(ss[1]);
            QuerySnapshot? updateCash = await db.Collection("Account")
                .WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid))
                .GetSnapshotAsync();

            foreach (DocumentSnapshot? item in updateCash.Documents)
            {
                _ = await item.Reference.UpdateAsync("Wallet", item.GetValue<double>("Wallet") + cash);
            }
            _ = await snapshot.Documents[0].Reference.UpdateAsync(new Dictionary<string, object> { { ss[1], 0 } });

        }
        catch
        {
            return BadRequest("Salary Failed");
        }

        return Ok("Done check your Wallet");
    }
    /// <summary>
    /// It takes a list of strings, the first string is the movie id, the second string is the date, and
    /// the third string is the studio id.
    /// </summary>
    /// <param name="ss">List of strings</param>
    /// <returns>
    /// The cash for the month and year that is passed in.
    /// </returns>
    [HttpPost("Check")]
    public async Task<ActionResult> Check([FromBody] List<string> ss)
    {
        try
        {
            ss[1] = DateTime.Parse(ss[1], new CultureInfo("en-US")).ToString("MM yyyy");

            double snapshot = (await db.Collection("Movie").WhereEqualTo("MovieId", ss[0]).WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>(ss[1]);
            return Ok($"Cash for {ss[1]} is {snapshot}");
        }
        catch (Exception)
        {
            return Ok($"Cash for {ss[1]} is 0");
        }

    }
    /// <summary>
    /// It takes a dictionary of strings, and then checks if the user has enough money to send the amount
    /// of money they want to send. If they do, it sends the money to the email they provided
    /// </summary>
    /// <param name="dic">Dictionary<string, string></param>
    /// <returns>
    /// The error message
    /// </returns>
    [HttpPost("Salary")]
    public async Task<ActionResult> Salary([FromBody] Dictionary<string, string> dic)
    {

        QuerySnapshot snapshot = await db.Collection("Account")
            .WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid))
            .GetSnapshotAsync();
        foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
        {
            double cashTest = snapshotDocument.ConvertTo<AccountManagementModel>().Wallet;


            if (Convert.ToDouble(dic["Cash"]) > cashTest)
            {

                return BadRequest($"Lower than {cashTest}");
            }

            _ = await snapshotDocument.Reference.UpdateAsync(
                new Dictionary<string, dynamic> { { "Wallet", cashTest - Convert.ToDouble(dic["Cash"]) } });
        }
        string clientId = "AUiGr3FOSHrsVSTbFwS_NFq8g-fGt1ovVj0LY9f0D260rprZgDB-VL8-Ww0Gwz4bsShhLz0YG8iawmjf";
        string secret = "EONxXLT9WLegeVtXtnvqfXCaGbDGrn2pyeLtB_ngG10Dq8Wu-Ay8JpmIvEGrH3fN4dA0dNhcPdTXjwgk";

        SandboxEnvironment environment = new(clientId, secret);
        PayPalHttpClient client = new(environment);
        CreatePayoutRequest createPayoutRequest = new()
        {
            SenderBatchHeader =
                new SenderBatchHeader
                {
                    EmailMessage = $"Congrats on recieving {dic["Cash"]}$",
                    EmailSubject = "You recieved a payout!!"
                },
            Items = new List<PayoutItem> {
                    new() {
                        RecipientType = "EMAIL",
                        Amount = new Currency {CurrencyCode = "USD", Value = dic["Cash"]},
                        Receiver = dic["Email"]
                    }
                }
        };
        try
        {
            PayoutsPostRequest request = new();
            _ = request.RequestBody(createPayoutRequest);

            await client.Execute(request);


            //CreatePayoutResponse result = response.Result<CreatePayoutResponse>();

            return Ok("Success");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
    /// <summary>
    /// It deletes a movie from the database and deletes the movie and image from the storage
    /// </summary>
    /// <param name="MovieModel">This is the model that I'm using to store the data.</param>
    /// <returns>
    /// The movie is being returned.
    /// </returns>
    [HttpPost("DeleteMovie")]

    public async Task<ActionResult> DeleteMovie([FromBody] MovieModel movie)
    {

        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", movie.MovieId).WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid));

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
            }
            catch
            {
                return BadRequest("Not success");

            }

            try
            {
                Task delete = new FirebaseStorage("movie2-e3c7b.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = async () => await Task.FromResult(User.FindFirstValue("Token"))
                    }).Child(movie.StudioId).Child(movie.MovieId).Child("Movie")
                .DeleteAsync();
                await delete;
            }
            catch
            {
                return BadRequest("Not success");

            }

            _ = await snapshotDocument.Reference.DeleteAsync();
        }
        return Ok("Success");
    }
}
