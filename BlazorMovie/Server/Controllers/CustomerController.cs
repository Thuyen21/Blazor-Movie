using BlazorMovie.Shared;
using Braintree;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorMovie.Server.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Customer")]
public class CustomerController : Controller
{
    private readonly FirestoreDb db;
    private readonly Censor censor;
    public CustomerController(Censor censor, FirestoreDb db)
    {
        this.censor = censor;
        this.db = db;
    }
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
            List<MovieModel> myFoo = new();
            usersRef = usersRef.Offset(index * 5).Limit(5);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                myFoo.Add(document.ConvertTo<MovieModel>());
            }

            return await Task.FromResult(myFoo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpGet("Watch/{Id}")]
    public async Task<ActionResult<MovieModel>> Watch(string Id)
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
    //[HttpGet("Deposit")]
    //public IActionResult Deposit(string Cash, string clientToken)
    //{
    //    if (Cash == null)
    //    {
    //        return View();
    //    }

    //    BraintreeGateway gateway = new()
    //    {
    //        Environment = Braintree.Environment.SANDBOX,
    //        MerchantId = "ts2vvzhy3dzc2dzc",
    //        PublicKey = "mg2whqyw68xxm3g9",
    //        PrivateKey = "b12f9762c30b546c29e5172fd3729c0d"
    //    };
    //    if (clientToken == null)
    //    {
    //        clientToken = gateway.ClientToken.Generate();
    //        ViewBag.clientToken = clientToken;
    //        ViewBag.Cash = Cash;
    //        return View();
    //    }

    //    return View();
    //}
    [HttpGet("Deposit")]
    public async Task<ActionResult<char[]>> Deposit()
    {
        try
        {
            string clientToken;
            BraintreeGateway gateway = new()
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "ts2vvzhy3dzc2dzc",
                PublicKey = "mg2whqyw68xxm3g9",
                PrivateKey = "b12f9762c30b546c29e5172fd3729c0d"
            };
            clientToken = gateway.ClientToken.Generate();
            return await Task.FromResult(Ok(clientToken.ToCharArray()));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }
    [HttpPost("DoCard")]
    public async Task<ActionResult> DoCard([FromBody] List<string> d)
    {
        string nonce = d[0];
        string cash = d[1];
        TransactionRequest request = new()
        {
            Amount = Convert.ToDecimal(cash),
            PaymentMethodNonce = nonce,
            Options = new TransactionOptionsRequest { SubmitForSettlement = true }
        };
        BraintreeGateway gateway = new()
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "ts2vvzhy3dzc2dzc",
            PublicKey = "mg2whqyw68xxm3g9",
            PrivateKey = "b12f9762c30b546c29e5172fd3729c0d"
        };
        Result<Braintree.Transaction> result = gateway.Transaction.Sale(request);
        if (result.IsSuccess())
        {
            Braintree.Transaction transaction = result.Target;

            Query usersRef = db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid));

            QuerySnapshot snapshotAsync = await usersRef.GetSnapshotAsync();


            foreach (DocumentSnapshot snapshotAsyncDocument in snapshotAsync.Documents)
            {
                _ = await snapshotAsyncDocument.Reference.UpdateAsync(
                    new Dictionary<string, dynamic> {
                            {
                                "Wallet", Convert.ToDouble(transaction.Amount) +
                                          snapshotAsyncDocument.ConvertTo<AccountManagementModel>().Wallet
                            }
                    });
            }
            return Ok("Success");
        }
        else if (result.Transaction != null)
        {
            //Braintree.Transaction transaction = result.Transaction;

            //ModelState.AddModelError(string.Empty, "Error processing transaction:");
            //ModelState.AddModelError(string.Empty, "  Status: " + transaction.Status);
            //ModelState.AddModelError(string.Empty, "  Code: " + transaction.ProcessorResponseCode);
            //ModelState.AddModelError(string.Empty, "  Text: " + transaction.ProcessorResponseText);
            //string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            //return Redirect($"{hostname}/Deposit/Error processing transaction");
            return BadRequest("Error processing transaction");
        }
        else
        {
            foreach (ValidationError error in result.Errors.DeepAll())
            {
                //ModelState.AddModelError(string.Empty, "Attribute: " + error.Attribute);
                //ModelState.AddModelError(string.Empty, "  Code: " + error.Code);
                //ModelState.AddModelError(string.Empty, "  Message: " + error.Message);

                //string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                //return Redirect($"{hostname}/Deposit/{error.Message}");
                return BadRequest(error.Message);
            }
        }

        return Ok();
    }
    [HttpPost("DoPayPal")]
    public async Task<ActionResult> DoPaypal([FromBody] List<string> d)
    {
        string nonce = d[0];
        string cash = d[1];
        BraintreeGateway gateway = new()
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "ts2vvzhy3dzc2dzc",
            PublicKey = "mg2whqyw68xxm3g9",
            PrivateKey = "b12f9762c30b546c29e5172fd3729c0d"
        };

        TransactionRequest request = new()
        {
            Amount = Convert.ToDecimal(cash),
            PaymentMethodNonce = nonce,
            OrderId = "Mapped to PayPal Invoice Number",
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true,
                PayPal = new TransactionOptionsPayPalRequest
                {
                    CustomField = "PayPal custom field",
                    Description = "Description for PayPal email receipt"
                }
            }
        };

        Result<Braintree.Transaction> result = gateway.Transaction.Sale(request);
        if (result.IsSuccess())
        {
            Braintree.Transaction transaction = result.Target;

            Query usersRef = db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid));

            QuerySnapshot snapshotAsync = await usersRef.GetSnapshotAsync();


            foreach (DocumentSnapshot snapshotAsyncDocument in snapshotAsync.Documents)
            {
                _ = await snapshotAsyncDocument.Reference.UpdateAsync(
                    new Dictionary<string, dynamic> {
                            {
                                "Wallet", Convert.ToDouble(transaction.Amount) +
                                          snapshotAsyncDocument.ConvertTo<AccountManagementModel>().Wallet
                            }
                    });
            }

            //ModelState.AddModelError(string.Empty, "Success!: " + transaction.Id);
            //string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            //return Redirect($"{hostname}/Deposit/Success");
            return Ok("Success");
        }
        else if (result.Transaction != null)
        {
            //Braintree.Transaction transaction = result.Transaction;

            //ModelState.AddModelError(string.Empty, "Error processing transaction:");
            //ModelState.AddModelError(string.Empty, "  Status: " + transaction.Status);
            //ModelState.AddModelError(string.Empty, "  Code: " + transaction.ProcessorResponseCode);
            //ModelState.AddModelError(string.Empty, "  Text: " + transaction.ProcessorResponseText);
            //string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            //return Redirect($"{hostname}/Deposit/Error processing transaction");
            return BadRequest("Error processing transaction");
        }
        else
        {
            foreach (ValidationError error in result.Errors.DeepAll())
            {
                //ModelState.AddModelError(string.Empty, "Attribute: " + error.Attribute);
                //ModelState.AddModelError(string.Empty, "  Code: " + error.Code);
                //ModelState.AddModelError(string.Empty, "  Message: " + error.Message);
                //string hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                //return Redirect($"{hostname}/Deposit/{error.Message}");

                return BadRequest(error.Message);
            }
        }
        return Ok();
    }
    [HttpGet("VipCheck")]
    public async Task<ActionResult<char[]>> VipCheck()
    {
        QuerySnapshot vipCheck = await db.Collection("Vip").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync();
        return vipCheck.Documents.Count == 0
            ? (ActionResult<char[]>)await Task.FromResult("Not Vip".ToCharArray())
            : vipCheck.Documents[0].GetValue<DateTime>("Time") < DateTime.UtcNow
                ? (ActionResult<char[]>)await Task.FromResult("Expires Vip".ToCharArray())
                : (ActionResult<char[]>)await Task.FromResult(vipCheck.Documents[0].GetValue<DateTime>("Time").ToString().ToCharArray());
    }
    [HttpPost("BuyVip")]
    public async Task<ActionResult> BuyVip([FromBody] VipModel vip)
    {

        CollectionReference collection = db.Collection("Buy");
        if (vip.Choose == 0 && vip.Id != null)
        {
            if ((await db.Collection("Account").
                        WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>("Wallet") < 4.99)
            {
                return BadRequest("Not enough cash");
            }
            if ((await db.Collection("Buy").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).WhereEqualTo("MovieId", vip.Id).GetSnapshotAsync()).Documents.Count > 0)
            {
                return BadRequest("Already buy this movie");
            }
            Task<DocumentReference>? buyTask = Task.Run(async () => await db.Collection("Buy").AddAsync(new Dictionary<string, dynamic>() { { "MovieId", vip.Id }, { "User", User.FindFirstValue(ClaimTypes.Sid) }, { "Time", DateTime.UtcNow } }));

            Task<WriteResult>? cashTask = Task.Run(async () => await (await db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic> { { "Wallet", (await db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>("Wallet") - 4.99 } }));
            Task addMoneyMovie = Task.Run(async () =>
            {
                QuerySnapshot? snap = await db.Collection("Movie").WhereEqualTo("MovieId", vip.Id).GetSnapshotAsync();
                _ = await snap.Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic> { { DateTime.UtcNow.ToString("MM yyyy"), snap.Documents[0].GetValue<double>(DateTime.UtcNow.ToString("MM yyyy")) + 4 } });
            });
            await Task.WhenAll(buyTask, cashTask, addMoneyMovie);
        }
        else
        {
            if (vip.Choose is 1 or 3 or 6 or 12)
            {
                double minus = new();
                switch (vip.Choose)
                {
                    case 1:
                        minus = 9.99;
                        break;
                    case 3:
                        minus = 27.49;
                        break;
                    case 6:
                        minus = 49.99;
                        break;
                    case 12:
                        minus = 97.49;
                        break;
                    default:
                        break;
                }
                if ((await db.Collection("Account").
                        WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>("Wallet") < minus)
                {
                    return BadRequest("Not enough cash");
                }
                QuerySnapshot vipCheck = await db.Collection("Vip").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync();
                if (vipCheck.Documents.Count == 0)
                {
                    Task<DocumentReference>? buyTask = Task.Run(async () => await db.Collection("Vip").AddAsync(new Dictionary<string, dynamic>() { { "User", User.FindFirstValue(ClaimTypes.Sid) }, { "Time", DateTime.UtcNow.AddMonths(vip.Choose) } }));
                    Task<WriteResult>? cashTask = Task.Run(async () => await (await db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).
                        Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic>{{"Wallet", (await db.Collection("Account").
                            WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>("Wallet") - minus} }));
                    await Task.WhenAll(buyTask, cashTask);
                }
                else
                {
                    DateTime dateCheck = vipCheck.Documents[0].GetValue<DateTime>("Time");
                    if (dateCheck < DateTime.UtcNow)
                    {
                        Task<WriteResult>? buyTask = Task.Run(async () => await vipCheck.Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic> { { "Time", DateTime.UtcNow.AddMonths(vip.Choose) } }));
                        Task<WriteResult>? cashTask = Task.Run(async () => await (await db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).
                        Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic>{{"Wallet", (await db.Collection("Account").
                            WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>("Wallet") - minus} }));
                        _ = await Task.WhenAll(buyTask, cashTask);
                    }

                    else
                    {
                        Task<WriteResult>? buyTask = Task.Run(async () => await vipCheck.Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic> { { "Time", dateCheck.AddMonths(vip.Choose) } }));
                        Task<WriteResult>? cashTask = Task.Run(async () => await (await db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).
                        Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic>{{"Wallet", (await db.Collection("Account").
                            WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<double>("Wallet") - minus} }));
                        _ = await Task.WhenAll(buyTask, cashTask);
                    }
                }
            }
        }
        return Ok("Success");
    }
    [HttpPost("UserAgent")]
    public async Task<ActionResult> UserAgent([FromBody] string Device)
    {
        try
        {
            string check = (await db.Collection("Account").WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync()).Documents[0].GetValue<string>("UserAgent");
            return check != Device ? BadRequest() : Ok();
        }
        catch
        {
            return BadRequest();
        }

    }
    [HttpPost("View")]
    public async Task<ActionResult> Viewing([FromBody] string Id)
    {
        QuerySnapshot collectionView = await db.Collection("View").WhereEqualTo("Id", Id).WhereEqualTo("Viewer", User.FindFirstValue(ClaimTypes.Sid)).WhereGreaterThanOrEqualTo("Time", DateTime.UtcNow.AddHours(-0.5)).WhereLessThanOrEqualTo("Time", DateTime.UtcNow).GetSnapshotAsync();
        if (collectionView.Documents.Count == 0)
        {
            QuerySnapshot? vip = await db.Collection("Vip").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync();
            DateTime timeVip = vip.Documents[0].GetValue<DateTime>("Time");
            if (timeVip < DateTime.UtcNow)
            {
                return BadRequest();
            }
            DateTime time = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, timeVip.Day);
            collectionView = await db.Collection("View").WhereGreaterThanOrEqualTo("Time", time.ToUniversalTime()).WhereLessThan("Time", time.AddMonths(1).ToUniversalTime()).GetSnapshotAsync();
            _ = await db.Collection("View").AddAsync(new Dictionary<string, dynamic>() { { "Id", Id }, { "Viewer", User.FindFirstValue(ClaimTypes.Sid) }, { "Time", DateTime.UtcNow } });
            List<string> movie = new();
            foreach (DocumentSnapshot? item in collectionView.Documents)
            {
                movie.Add(item.GetValue<string>("Id"));
            }

            double oldCash = movie.Count == 0 ? 0 : 8 / (movie.Count() * 1.0);
            double newCash = 8 / ((movie.Count() * 1.0) + 1.0);
            QuerySnapshot? newUP = await db.Collection("Movie").WhereEqualTo("MovieId", Id).GetSnapshotAsync();
            double newUPCash = 0;
            try
            {
                newUPCash = newUP.Documents[0].GetValue<double>(time.ToString("MM yyyy"));
            }
            catch
            {
                newUPCash = 0;
            }
            _ = await newUP.Documents[0].Reference.UpdateAsync(new Dictionary<string, dynamic> { { time.ToString("MM yyyy"), newUPCash + newCash } });

            foreach (string? item2 in movie)
            {
                QuerySnapshot? movieSnapshot = await db.Collection("Movie").WhereEqualTo("MovieId", item2).GetSnapshotAsync();
                _ = Parallel.ForEach(movieSnapshot.Documents, async item3 =>
                {
                    double updateCash = 0;
                    try
                    {
                        updateCash = item3.GetValue<double>(time.ToString("MM yyyy"));
                    }
                    catch
                    {
                        updateCash = 0;
                    }
                    if (updateCash - oldCash + newCash >= 0)
                    {
                        _ = await item3.Reference.UpdateAsync(new Dictionary<string, dynamic> { { time.ToString("MM yyyy"), updateCash - oldCash + newCash } });
                    }
                    else
                    {
                        double hazz = 0;
                        try
                        {
                            hazz = item3.GetValue<double>(time.AddMonths(1).ToString("MM yyyy"));
                        }
                        catch
                        {
                            hazz = 0;
                        }
                        double hazzz = hazz + updateCash - oldCash + newCash;
                        _ = await item3.Reference.UpdateAsync(new Dictionary<string, dynamic> { { time.ToString("MM yyyy"), 0 } });
                        _ = await item3.Reference.UpdateAsync(new Dictionary<string, dynamic> { { time.AddMonths(1).ToString("MM yyyy"), hazzz } });
                    }
                });
            }
            return Ok();
        }
        else
        {
            return Ok();
        }

    }
    [HttpGet("CanWatch/{Id}")]
    public async Task<ActionResult<bool>> CanWatch(string Id)
    {

        QuerySnapshot collectionCheckVip = await db.Collection("Vip").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).OrderByDescending("Time").Limit(1).GetSnapshotAsync();

        if (collectionCheckVip.Documents.Count != 0)

        {
            if (collectionCheckVip.Documents[0].GetValue<DateTime>("Time") >= DateTime.UtcNow)
            {
                return await Task.FromResult(true);
            }
            else
            {
                QuerySnapshot collectionCheckBuy = await db.Collection("Buy").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).WhereEqualTo("MovieId", Id).GetSnapshotAsync();
                return collectionCheckBuy.Documents.Count == 0 ? (ActionResult<bool>)await Task.FromResult(false) : (ActionResult<bool>)await Task.FromResult(true);
            }

        }
        else
        {
            QuerySnapshot collectionCheckBuy = await db.Collection("Buy").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).WhereEqualTo("MovieId", Id).GetSnapshotAsync();
            return collectionCheckBuy.Documents.Count == 0 ? (ActionResult<bool>)await Task.FromResult(false) : (ActionResult<bool>)await Task.FromResult(true);
        }
    }
    [HttpGet("Comment/{Id?}/{index:int:min(0)}")]
    public async Task<ActionResult<List<CommentModel>>> Comment(string? Id, int index)
    {
        Query commentSend = db.Collection("Comment").WhereEqualTo("MovieId", Id).OrderByDescending("Time").Offset(index * 5).Limit(5);
        QuerySnapshot commentSnapshot = await commentSend.GetSnapshotAsync();
        List<CommentModel> commentList = new();
        string? user = User.FindFirstValue(ClaimTypes.Sid);
        foreach (DocumentSnapshot item in commentSnapshot.Documents)
        {
            CommentModel commentConvert = item.ConvertTo<CommentModel>();
            commentConvert.CommentText = censor.CensorText(commentConvert.CommentText);
            QuerySnapshot? temp = await db.Collection("CommentAcction").WhereEqualTo("User", user).WhereEqualTo("CommentId", commentConvert.Id).GetSnapshotAsync();
            commentConvert.Is = temp.Documents.Count != 0 ? temp.Documents[0].GetValue<string>("Action") : "No";
            commentList.Add(commentConvert);
        }
        return await Task.FromResult(commentList);
    }
    [HttpPost("Acomment")]
    public async Task<ActionResult> Acomment([FromBody] CommentModel comment)
    {
        try
        {
            comment.Email = User.FindFirstValue(ClaimTypes.Email);
            DocumentReference commentUp = await db.Collection("Comment").AddAsync(comment);
            _ = await commentUp.UpdateAsync(new Dictionary<string, dynamic> { { "Id", commentUp.Id } });
            return Ok("Commented");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("Ac/{Id}")]
    public async Task<ActionResult> Ac([FromBody] string ac, string Id)
    {

        if ((await db.Collection("CommentAcction").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).WhereEqualTo("CommentId", Id).GetSnapshotAsync()).Documents.Count == 0)
        {
            if (ac == "Like")
            {
                Task<DocumentReference>? Task1 = Task.Run(async () => await db.Collection("CommentAcction").AddAsync(new Dictionary<string, dynamic>() { { "Action", "Like" }, { "CommentId", Id }, { "User", User.FindFirstValue(ClaimTypes.Sid) } }));
                QuerySnapshot? temp = await db.Collection("Comment").WhereEqualTo("Id", Id).GetSnapshotAsync();
                Task<WriteResult>? Task2 = Task.Run(async () => await temp.Documents[0].Reference.UpdateAsync("Like", temp.Documents[0].ConvertTo<CommentModel>().Like + 1));
                await Task.WhenAll(Task1, Task2);
            }
            if (ac == "DisLike")
            {
                Task<DocumentReference>? Task1 = Task.Run(async () => await db.Collection("CommentAcction").AddAsync(new Dictionary<string, dynamic>() { { "Action", "DisLike" }, { "CommentId", Id }, { "User", User.FindFirstValue(ClaimTypes.Sid) } }));
                QuerySnapshot? temp = await db.Collection("Comment").WhereEqualTo("Id", Id).GetSnapshotAsync();
                Task<WriteResult>? Task2 = Task.Run(async () => await temp.Documents[0].Reference.UpdateAsync("DisLike", temp.Documents[0].ConvertTo<CommentModel>().DisLike + 1));
                await Task.WhenAll(Task1, Task2);
            }
        }
        else
        {
            QuerySnapshot deleteCommentAction = await db.Collection("CommentAcction").WhereEqualTo("User", User.FindFirstValue(ClaimTypes.Sid)).WhereEqualTo("CommentId", Id).GetSnapshotAsync();
            foreach (DocumentSnapshot item in deleteCommentAction.Documents)
            {

                if (item.GetValue<string>("Action") == ac)
                {
                    _ = await item.Reference.DeleteAsync();
                    QuerySnapshot? temp = await db.Collection("Comment").WhereEqualTo("Id", Id).GetSnapshotAsync();
                    _ = ac == "Like"
                        ? await temp.Documents[0].Reference.UpdateAsync("Like", temp.Documents[0].ConvertTo<CommentModel>().Like - 1)
                        : await temp.Documents[0].Reference.UpdateAsync("DisLike", temp.Documents[0].ConvertTo<CommentModel>().DisLike - 1);

                }
                else
                {
                    if (ac == "Like")
                    {
                        Task<WriteResult>? Task1 = Task.Run(async () => await item.Reference.UpdateAsync(new Dictionary<string, dynamic>() { { "Action", "Like" } }));
                        QuerySnapshot? temp = await db.Collection("Comment").WhereEqualTo("Id", Id).GetSnapshotAsync();
                        Task<WriteResult>? Task2 = Task.Run(async () => await temp.Documents[0].Reference.UpdateAsync("Like", temp.Documents[0].ConvertTo<CommentModel>().Like + 1));
                        Task<WriteResult>? Task3 = Task.Run(async () => await temp.Documents[0].Reference.UpdateAsync("DisLike", temp.Documents[0].ConvertTo<CommentModel>().DisLike - 1));
                        _ = await Task.WhenAll(Task1, Task2, Task3);
                    }
                    if (ac == "DisLike")
                    {
                        Task<WriteResult>? Task1 = Task.Run(async () => await item.Reference.UpdateAsync(new Dictionary<string, dynamic>() { { "Action", "DisLike" } }));
                        QuerySnapshot? temp = await db.Collection("Comment").WhereEqualTo("Id", Id).GetSnapshotAsync();
                        Task<WriteResult>? Task2 = Task.Run(async () => await temp.Documents[0].Reference.UpdateAsync("Like", temp.Documents[0].ConvertTo<CommentModel>().Like - 1));
                        Task<WriteResult>? Task3 = Task.Run(async () => await temp.Documents[0].Reference.UpdateAsync("DisLike", temp.Documents[0].ConvertTo<CommentModel>().DisLike + 1));
                        _ = await Task.WhenAll(Task1, Task2, Task3);
                    }
                }

            }

        }
        return Ok();
    }
}

