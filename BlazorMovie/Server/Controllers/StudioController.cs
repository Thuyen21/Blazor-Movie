using BlazorMovie.Shared;
using BlazorMovie_Server;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayoutsSdk.Core;
using PayoutsSdk.Payouts;
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
    [HttpPost("EditMovie")]
    public async Task<ActionResult> EditMovie([FromBody] MovieModel movie)
    {
        Query collection = db.Collection("Movie").WhereEqualTo("MovieId", movie.MovieId);
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        movie.PremiereDate = movie.PremiereDate.ToUniversalTime();
        Dictionary<string, dynamic> dictionary = movie.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(movie, null));

        _ = Parallel.ForEach(snapshot.Documents, async snapshotDocument =>
        {
            _ = await snapshotDocument.Reference.UpdateAsync(dictionary);
        });

        return Ok("Success");
    }
    [HttpGet("Done")]
    public async Task<ActionResult> Done()
    {
        return await Task.FromResult(View());
    }
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
    [HttpPost("MovieUpload/{MovieId}/{StudioId}")]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<ActionResult> MovieUpload(string StudioId, string MovieId, IFormFile ImageFileUp, IFormFile MovieFileUp)
    {
        StudioId = User.FindFirstValue(ClaimTypes.Sid);
        if (ImageFileUp != null)
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

                    _ = await task;

                    fileStream.Close();
                }
            }

        }
        if (MovieFileUp != null)
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

                    _ = await task;

                    fileStream.Close();
                }
            }

        }
        string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        string redirect = hostname;
        return Redirect(redirect);
    }
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

    [HttpGet("PayCheck/{Id}/{Start}")]
    public async Task<ActionResult<List<double>>> PayCheck(string Id, string Start)
    {
        DateTime StartDate = DateTime.Parse(Start).AddHours(12).ToUniversalTime();
        DateTime EndDate = StartDate.AddDays(1);
        try
        {
            double viewCount = (await db.Collection("View").WhereGreaterThanOrEqualTo("Time", StartDate).WhereLessThanOrEqualTo("Time", EndDate).GetSnapshotAsync()).Documents.Count;
            if (viewCount == 0)
            {
                double buy0 = (await db.Collection("Buy").WhereEqualTo("MovieId", Id).WhereGreaterThanOrEqualTo("Time", StartDate).WhereLessThanOrEqualTo("Time", EndDate).GetSnapshotAsync()).Documents.Count;
                double m0 = buy0 * 4.49;
                List<double> result0 = new()
                {
                    0,
                    buy0
                };
                return result0;
            }
            double viewt = (await db.Collection("View").WhereEqualTo("Id", Id).WhereGreaterThanOrEqualTo("Time", StartDate).WhereLessThanOrEqualTo("Time", EndDate).GetSnapshotAsync()).Documents.Count;
            double buy = (await db.Collection("Buy").WhereEqualTo("MovieId", Id).WhereGreaterThanOrEqualTo("Time", StartDate).WhereLessThanOrEqualTo("Time", EndDate).GetSnapshotAsync()).Documents.Count;
            double vip = (await db.Collection("Vip").WhereGreaterThanOrEqualTo("Time", StartDate).GetSnapshotAsync()).Documents.Count;

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
    [HttpGet("CommentStatus/{Id}/{Start}")]
    public async Task<ActionResult<List<int>>> CommentStatus(string Id, string Start)
    {
        try
        {
            DateTime StartDate = DateTime.Parse(Start).AddHours(12);
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
    [HttpPost("SalaryMovie")]
    public async Task<ActionResult> SalaryMovie([FromBody] List<string> ss)
    {

        if (DateTime.Parse(ss[1]).Month > DateTime.UtcNow.Month - 1)
        {
            return BadRequest("Cant Salary In lower than month now -1");
        }
        try
        {
            ss[1] = DateTime.Parse(ss[1]).ToString("MM yyyy");

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

        }

        return Ok("Done check your Wallet");
    }
    [HttpPost("Check")]
    public async Task<ActionResult> Check([FromBody] List<string> ss)
    {
        try
        {
            ss[1] = DateTime.Parse(ss[1]).ToString("MM yyyy");

            double snapshot = (await db.Collection("Movie").WhereEqualTo("MovieId", ss[0]).WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>(ss[1]);
            return Ok($"Cash for {ss[1]} is {snapshot}");
        }
        catch (Exception)
        {
            return Ok($"Cash for {ss[1]} is 0");
        }

    }
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

            PayPalHttp.HttpResponse response = await client.Execute(request);


            CreatePayoutResponse result = response.Result<CreatePayoutResponse>();

            return Ok("Success");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
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


            }

            _ = await snapshotDocument.Reference.DeleteAsync();
            return Ok("Success");
        }
        return BadRequest("Not success");
    }
}
