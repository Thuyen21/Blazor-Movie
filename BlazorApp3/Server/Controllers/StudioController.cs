using BlazorApp3.Shared;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PayoutsSdk.Core;
using PayoutsSdk.Payouts;
using System.Reflection;
using System.Security.Claims;

namespace BlazorApp3.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = "Studio")]
    public class StudioController : Controller
    {
        [HttpGet("Index/{searchString}/{sortOrder}")]
        public async Task<ActionResult<List<MovieModel>>> Index(string searchString, string sortOrder)
        {
            try
            {
                FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
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
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
            CollectionReference collection = db.Collection("Movie");
            try
            {
                movie.PremiereDate = movie.PremiereDate.ToUniversalTime();
                movie.StudioId = User.FindFirstValue(ClaimTypes.Sid);
                DocumentReference MovieId = await collection.AddAsync(movie);
                await MovieId.UpdateAsync(new Dictionary<string, object> { { "MovieId", MovieId.Id } });
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
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
            Query collection = db.Collection("Movie").WhereEqualTo("MovieId", Id).WhereEqualTo("StudioId", User.FindFirstValue(ClaimTypes.Sid));

            QuerySnapshot snapshot = await collection.GetSnapshotAsync();
            MovieModel movie = new();

            if (snapshot.Documents.Count < 1)
            {
                return null;
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
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
            Query collection = db.Collection("Movie").WhereEqualTo("MovieId", movie.MovieId);
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();

            movie.PremiereDate = movie.PremiereDate.ToUniversalTime();

            Dictionary<string, object> dictionary = movie.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(movie, null));

            foreach (DocumentSnapshot snapshotDocument in snapshot.Documents)
            {
                await snapshotDocument.Reference.UpdateAsync(dictionary);
            }
            return Ok("Success");
        }

        [HttpGet("MovieUpload/{MovieId}")]
        public async Task<ActionResult> MovieUpload(string MovieId)
        {
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
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

            return Redirect(hostname + "/EditMovieStudio/" + MovieId);
        }
        [HttpGet("Comment/{Id}")]
        public async Task<ActionResult<List<CommentModel>>> Comment(string Id)
        {
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");

            Query commentSend = db.Collection("Comment").WhereEqualTo("MovieId", Id).OrderByDescending("Time");
            QuerySnapshot commentSnapshot = await commentSend.GetSnapshotAsync();
            List<CommentModel> commentList = new List<CommentModel>();

            foreach (DocumentSnapshot item in commentSnapshot.Documents)
            {
                CommentModel commentConvert = item.ConvertTo<CommentModel>();
                int like = (await db.Collection("CommentAcction").WhereEqualTo("CommentId", item.Id).WhereEqualTo("Action", "Like").GetSnapshotAsync()).Documents.Count;
                int Dislike = (await db.Collection("CommentAcction").WhereEqualTo("CommentId", item.Id).WhereEqualTo("Action", "DisLike").GetSnapshotAsync()).Documents.Count;
                commentList.Add(new CommentModel() { Id = commentConvert.Id, Email = commentConvert.Email, MovieId = commentConvert.MovieId, Time = commentConvert.Time, CommentText = commentConvert.CommentText, Like = like, DisLike = Dislike });
            }

            return await Task.FromResult(commentList);
        }
        [HttpGet("CommentStatus/{Id}/{check}")]
        public async Task<ActionResult<List<int>>> CommentStatus(string Id, string check)
        {
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");

            Query commentSend = db.Collection("Comment").WhereEqualTo("MovieId", Id).OrderByDescending("Time");
            QuerySnapshot commentSnapshot = await commentSend.GetSnapshotAsync();

            List<int> list = new();
            list.Add(0);
            list.Add(0);

            foreach (DocumentSnapshot item in commentSnapshot.Documents)
            {
                CommentModel commentConvert = item.ConvertTo<CommentModel>();

                try
                {
                    //Load sample data
                    MLModel.ModelInput sampleData = new MLModel.ModelInput
                    {
                        Review = commentConvert.CommentText
                    };
                    //Load model and predict output
                    MLModel.ModelOutput result = MLModel.Predict(sampleData);
                    if (result.Prediction == "positive")
                    {
                        list[0] = list[0] + 1;
                        await item.Reference.UpdateAsync(new Dictionary<string, object> { { "Prediction", "Positive" } });
                    }
                    else
                    {
                        list[1] = list[1] + 1;
                        await item.Reference.UpdateAsync(new Dictionary<string, object> { { "Prediction", "Negative" } });
                    }
                }
                catch (Exception)
                {

                }



            }

            return list;
        }
        [HttpGet("CommentStatus/{Id}")]
        public async Task<ActionResult<List<int>>> CommentStatus(string Id)
        {
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");

            Query commentSend = db.Collection("Comment").WhereEqualTo("MovieId", Id).OrderByDescending("Time");
            QuerySnapshot commentSnapshot = await commentSend.GetSnapshotAsync();

            List<int> list = new();
            list.Add(0);
            list.Add(0);
            MLModel.ModelInput sampleData = new MLModel.ModelInput();
            foreach (DocumentSnapshot item in commentSnapshot.Documents)
            {
                CommentModel commentConvert = item.ConvertTo<CommentModel>();

                if (commentConvert.Prediction == null)
                {
                    //Load sample data
                    sampleData.Review = commentConvert.CommentText;
                    //Load model and predict output
                    MLModel.ModelOutput result = MLModel.Predict(sampleData);
                    if (result.Prediction == "positive")
                    {
                        list[0] = list[0] + 1;
                        await item.Reference.UpdateAsync(new Dictionary<string, object> { { "Prediction", "Positive" } });
                    }
                    else
                    {
                        list[1] = list[1] + 1;
                        await item.Reference.UpdateAsync(new Dictionary<string, object> { { "Prediction", "Negative" } });
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
        [HttpPost("Salary")]
        public async Task<ActionResult> Salary([FromBody] Dictionary<string, string> dic)
        {
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
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

                await snapshotDocument.Reference.UpdateAsync(
                    new Dictionary<string, object> { { "Wallet", cashTest - Convert.ToDouble(dic["Cash"]) } });
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
                request.RequestBody(createPayoutRequest);

                PayPalHttp.HttpResponse response = await client.Execute(request);


                CreatePayoutResponse result = response.Result<CreatePayoutResponse>();

                return Ok("Success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
