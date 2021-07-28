using BlazorApp3.Shared;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using static BlazorApp3.Client.Pages.WatchCustomer;

namespace BlazorApp3.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Studio")]
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

                var snapshot = await usersRef.GetSnapshotAsync();


                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    myFoo.Add(document.ConvertTo<MovieModel>());
                }

                return await Task.FromResult(myFoo);
            }
            catch (Exception ex)
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
            Query collection = db.Collection("Movie").WhereEqualTo("MovieId", Id).WhereEqualTo("StudioId",User.FindFirstValue(ClaimTypes.Sid));

            QuerySnapshot snapshot = await collection.GetSnapshotAsync();
            MovieModel movie = new();

            if(snapshot.Documents.Count < 1)
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
            Query collection = db.Collection("Movie").WhereEqualTo("MovieId", MovieId).WhereEqualTo("StudioId",User.FindFirstValue(ClaimTypes.Sid));

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
                List<string> list = new List<string>();
                list.Add("image/bmp");
                list.Add("image/gif");
                list.Add("image/jpeg");
                list.Add("image/png");
                list.Add("image/svg+xml");
                list.Add("image/tiff");
                list.Add("image/webp");
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
                List<string> list = new List<string>();
                list.Add("video/x-msvideo");
                list.Add("video/mp4");
                list.Add("video/mpeg");
                list.Add("video/ogg");
                list.Add("video/mp2t");
                list.Add("video/webm");
                list.Add("video/3gpp");
                list.Add("video/3gpp2");
                list.Add("video/x-matroska");

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
    }
}
