using BlazorMovie.Shared;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BlazorMovie.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : Controller
{

    private readonly FirestoreDb db;
    private readonly FirebaseAuthClient client;
    public UserController(FirestoreDb db, FirebaseAuthConfig config)
    {
        this.db = db;
        client = new FirebaseAuthClient(config);
    }

    private UserCredential? userCredential;

    /// <summary>
    /// It takes in a user's email and password, checks if the user exists in the database, if the user
    /// exists, it creates a claims identity and signs the user in
    /// </summary>
    /// <param name="LogInModel"></param>
    /// <returns>
    /// The token is being returned.
    /// </returns>
    [HttpPost("login")]
    public async Task<ActionResult> LogIn([FromBody] LogInModel logIn)
    {
        try
        {
            userCredential = await client.SignInWithEmailAndPasswordAsync(logIn.Email, logIn.Password);
        }
        catch
        {
            return BadRequest("Wrong email or password");
        }

        User user = userCredential.User;
        Query usersRef = db.Collection("Account").WhereEqualTo("Id", user.Uid);
        QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
        AccountManagementModel acc = new();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            _ = await document.Reference.UpdateAsync("UserAgent", logIn.UserAgent);
            acc = document.ConvertTo<AccountManagementModel>();
        }
        ClaimsIdentity claimsIdentity = new(new[] {
                new Claim(ClaimTypes.Email, logIn.Email?? string.Empty),
                    new Claim(ClaimTypes.Sid, user.Uid?? string.Empty),
                    new Claim(ClaimTypes.Name, acc.Name?? string.Empty),
                    new Claim(ClaimTypes.Role, acc.Role?? string.Empty),
                    new Claim(ClaimTypes.DateOfBirth, acc.DateOfBirth.ToShortDateString()),
                    new Claim("Token", await client.User.GetIdTokenAsync(true))

            }, "serverAuth");
        //create claimsPrincipal
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
        //Sign In User
        await HttpContext.SignInAsync(claimsPrincipal);
        return Ok();
    }
    /// <summary>
    /// It signs out the user from the IdentityServer4 server and the client application
    /// </summary>
    /// <returns>
    /// The user is being returned.
    /// </returns>
    [Authorize]
    [HttpGet("Logout")]
    public async Task<ActionResult> LogOut()
    {
        client.SignOut();
        await HttpContext.SignOutAsync();
        return Ok();
    }
    /// <summary>
    /// It gets the current user's information from the database and returns it to the client
    /// </summary>
    /// <returns>
    /// The current user's information.
    /// </returns>
    [Authorize]
    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<AccountManagementModel>> GetCurrentUser()
    {
        AccountManagementModel acc = new();
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            string? Id = User.FindFirst(ClaimTypes.Sid)?.Value;
            Query usersRef = db.Collection("Account").WhereEqualTo("Id", Id);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            foreach (DocumentSnapshot VARIABLE in snapshot.Documents)
            {
                acc = VARIABLE.ConvertTo<AccountManagementModel>();
            }
        }
        return await Task.FromResult(acc);
    }
    /// <summary>
    /// It takes a user's current email and password, and a new email, and changes the user's email to
    /// the new email
    /// </summary>
    /// <param name="ChangeEmailModel"></param>
    /// <returns>
    /// The user's email address.
    /// </returns>
    [Authorize]
    [HttpPost("ChangeEmail")]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailModel changeEmailModel)
    {
        changeEmailModel.Email = changeEmailModel.Email?.ToLower();
        try
        {
            userCredential =
                    await client.SignInWithEmailAndPasswordAsync(User.FindFirst(ClaimTypes.Email)?.Value,
                        changeEmailModel.Password);
            UserCredential newUserCredentiall = userCredential;
            newUserCredentiall.AuthCredential = EmailProvider.GetCredential(changeEmailModel.Email, changeEmailModel.Password);
            _ = await newUserCredentiall.User.LinkWithCredentialAsync(userCredential.AuthCredential);
            QuerySnapshot snapshot = await db.Collection("Account").WhereEqualTo("Id", userCredential.User.Uid)
                .GetSnapshotAsync();
            Dictionary<string, dynamic?> update = new() { { "Email", changeEmailModel.Email } };
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                _ = await document.Reference.UpdateAsync(update);
            }
            ClaimsIdentity identity = new(User.Identity);
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Email));
            identity.AddClaim(new Claim(ClaimTypes.Email, changeEmailModel.Email ?? string.Empty));
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// The function takes the email address of the user and sends a reset password email to the user
    /// </summary>
    /// <param name="ResetPasswordModel"></param>
    /// <returns>
    /// The result of the operation.
    /// </returns>
    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel resetPassword)
    {
        try
        {
            await client.ResetEmailPasswordAsync(resetPassword.Email);
            return Ok("An email has sent, Check your Email, please.");
        }
        catch (FirebaseAuthHttpException ex)
        {
            return BadRequest(ex.Reason.ToString());
        }
    }
    /// <summary>
    /// It gets the user's profile information from the database and returns it to the user
    /// </summary>
    /// <returns>
    /// The AccountManagementModel object is being returned.
    /// </returns>
    [Authorize]
    [HttpGet("Profile")]
    public Task<ActionResult<AccountManagementModel>> Profile()
    {
        //AccountManagementModel acc = new();
        //if (User.Identity is not null && User.Identity.IsAuthenticated)
        //{
        //    string? Id = User.FindFirst(ClaimTypes.Sid)?.Value;
        //    Query usersRef = db.Collection("Account").WhereEqualTo("Id", Id);
        //    QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
        //    foreach (DocumentSnapshot VARIABLE in snapshot.Documents)
        //    {
        //        acc = VARIABLE.ConvertTo<AccountManagementModel>();
        //    }
        //}
        return GetCurrentUser();
    }
    /// <summary>
    /// It takes in a model, queries the database for the user's account, updates the account with the
    /// model's data, and returns a success message
    /// </summary>
    /// <param name="AccountManagementModel"></param>
    /// <returns>
    /// The return type is an ActionResult.
    /// </returns>
    [Authorize]
    [HttpPost("EditProfile")]
    public async Task<ActionResult> EditProfile([FromBody] AccountManagementModel accountManagementModel)
    {
        try
        {
            QuerySnapshot snapshot = await db.Collection("Account")
                .WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync();
            Dictionary<string, dynamic?> update = new()
            {
                { "Name", accountManagementModel.Name },
                { "DateOfBirth", accountManagementModel.DateOfBirth.AddDays(1).ToUniversalTime() }
            };
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                _ = await document.Reference.UpdateAsync(update);
            }

            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    /// <summary>
    /// It creates a user with the email and password provided in the request body, then creates a
    /// document in the Account collection with the user's information
    /// </summary>
    /// <param name="SignUpModel"></param>
    /// <returns>
    /// The return type is an ActionResult.
    /// </returns>
    [HttpPost("SignUp")]
    public async Task<ActionResult> SignUp([FromBody] SignUpModel signUpModel)
    {
        if (signUpModel.Role is "Customer" or "Studio")
        {


            if (signUpModel.ConfirmPassword != signUpModel.Password)
            {
                return BadRequest("Password and Confirm Password are different");
            }
            signUpModel.Email = signUpModel.Email!.ToLower();
            try
            {
                try
                {
                    userCredential =
                        await client.CreateUserWithEmailAndPasswordAsync(signUpModel.Email, signUpModel.Password, signUpModel.Name);
                }
                catch (FirebaseAuthException ex)
                {

                    return BadRequest(ex.Reason.ToString());
                }
                User user = userCredential.User;
                CollectionReference docRef = db.Collection("Account");
                AccountManagementModel account = new()
                {
                    DateOfBirth = signUpModel.DateOfBirth.AddDays(1).ToUniversalTime(),
                    Email = signUpModel.Email,
                    Id = user.Uid,
                    Name = signUpModel.Name,
                    Role = signUpModel.Role,
                    Wallet = 0.0
                };
                _ = await docRef.AddAsync(account);
                ClaimsIdentity claimsIdentity = new(new[] {
                new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Sid, user.Uid),
                    new Claim(ClaimTypes.Name, account.Name ?? string.Empty),
                    new Claim(ClaimTypes.Role, account.Role),
                    new Claim(ClaimTypes.DateOfBirth, account.DateOfBirth.ToString()),
                    new Claim("Token", await user.GetIdTokenAsync())
            }, "serverAuth");
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        else
        {
            return BadRequest("Check role");
        }
    }

    /// <summary>
    /// If the user is authenticated, return the token, otherwise, create a custom token and return it
    /// </summary>
    /// <returns>
    /// A char array of the token.
    /// </returns>
    [HttpGet("GetToken")]
    public async Task<ActionResult<char[]>> GetToken()
    {
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            string? token = User.FindFirstValue("Token");
            char[] ch = new char[token?.Length ?? 0];
            for (int i = 0; i < token?.Length; i++)
            {
                ch[i] = token[i];
            }
            return await Task.FromResult(ch);
        }
        else
        {
            try
            {
                string? uid = "wgiW4ncipWSsHzaHIrHyaD0sSFL2";

                string customToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);

                char[] ch = new char[customToken.Length];
                for (int i = 0; i < customToken.Length; i++)
                {
                    ch[i] = customToken[i];
                }

                return await Task.FromResult(ch);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return BadRequest();
            }

        }

    }
    /// <summary>
    /// It takes a JSON object from the body of the request, and adds it to the database
    /// </summary>
    /// <param name="FeedbackMessageModel"></param>
    /// <returns>
    /// The return type is an ActionResult.
    /// </returns>
    [HttpPost("feedback")]
    public async Task<ActionResult> Feedback([FromBody] FeedbackMessageModel feedback)
    {
        _ = await db.Collection("Feedback").AddAsync(feedback);
        return Ok("Done");
    }

    /// <summary>
    /// It gets the top 10 most viewed movies in the last 24 hours
    /// </summary>
    /// <returns>
    /// A list of movies
    /// </returns>
    [HttpGet("Trending")]
    public async Task<ActionResult<List<MovieModel>>> Trending()
    {
        List<MovieModel> movies = new();


        QuerySnapshot? viewRef = await db.Collection("View").OrderBy("Time").StartAt(DateTime.Now.AddDays(-1).ToUniversalTime()).EndAt(DateTime.Now.ToUniversalTime()).GetSnapshotAsync();
        Dictionary<string, double> view = new();
        foreach (DocumentSnapshot? item in viewRef.Documents)
        {

            if (view.ContainsKey(item.GetValue<string>("Id")))
            {
                view[item.GetValue<string>("Id")] = view[item.GetValue<string>("Id")] + 1;
            }
            else
            {
                view.Add(item.GetValue<string>("Id"), 1);
            }
        }
        view = view.OrderBy(key => key.Value).ToDictionary(item => item.Key, item => item.Value);

        if (view.Count > 10)

        {
            for (int i = 0; i <= 10; i++)
            {
                movies.Add((await db.Collection("Movie").WhereEqualTo("MovieId", view.ElementAt(i).Key).GetSnapshotAsync()).Documents[0].ConvertTo<MovieModel>());

            }
        }
        else
        {
            foreach (KeyValuePair<string, double> item in view)
            {

                movies.Add((await db.Collection("Movie").WhereEqualTo("MovieId", item.Key).GetSnapshotAsync()).Documents[0].ConvertTo<MovieModel>());
            }
        }
        return await Task.FromResult(movies);


    }

}