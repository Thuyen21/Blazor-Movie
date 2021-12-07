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
    private readonly FirebaseAuthConfig config;
    public UserController(FirestoreDb db, FirebaseAuthConfig config)
    {
        this.db = db;
        client = new FirebaseAuthClient(config);
        this.config = config;
    }

    private static UserCredential? userCredential;

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
            await document.Reference.UpdateAsync("UserAgent", logIn.UserAgent);
            acc = document.ConvertTo<AccountManagementModel>();
        }
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, logIn.Email),
                    new Claim(ClaimTypes.Sid, user.Uid),
                    new Claim(ClaimTypes.Name, acc.Name),
                    new Claim(ClaimTypes.Role, acc.Role),
                    new Claim(ClaimTypes.DateOfBirth, acc.DateOfBirth.ToShortDateString()),
                    new Claim("Token", await client.User.GetIdTokenAsync(true))

            }, "serverAuth");
        //create claimsPrincipal
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        //Sign In User
        await HttpContext.SignInAsync(claimsPrincipal);
        return Ok();
    }
    [Authorize]
    [HttpGet("Logout")]
    public async Task<ActionResult> LogOut()
    {
        await client.SignOutAsync();
        await HttpContext.SignOutAsync();
        return Ok();
    }
    [Authorize]
    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<AccountManagementModel>> GetCurrentUser()
    {
        AccountManagementModel acc = new();
        if (User.Identity.IsAuthenticated)
        {
            string Id = User.FindFirst(ClaimTypes.Sid).Value;
            Query usersRef = db.Collection("Account").WhereEqualTo("Id", Id);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            foreach (DocumentSnapshot VARIABLE in snapshot.Documents)
            {
                acc = VARIABLE.ConvertTo<AccountManagementModel>();
            }
        }
        return await Task.FromResult(acc);
    }
    [Authorize]
    [HttpPost("ChangeEmail")]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailModel changeEmailModel)
    {
        changeEmailModel.Email = changeEmailModel.Email.ToLower();
        try
        {
            userCredential =
                    await client.SignInWithEmailAndPasswordAsync(User.FindFirst(ClaimTypes.Email).Value,
                        changeEmailModel.Password);
            UserCredential newUserCredentiall = userCredential;
            newUserCredentiall.AuthCredential = EmailProvider.GetCredential(changeEmailModel.Email, changeEmailModel.Password);
            await newUserCredentiall.User.LinkWithCredentialAsync(userCredential.AuthCredential);
            QuerySnapshot snapshot = await db.Collection("Account").WhereEqualTo("Id", userCredential.User.Uid)
                .GetSnapshotAsync();
            Dictionary<string, dynamic> update = new() { { "Email", changeEmailModel.Email } };
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                await document.Reference.UpdateAsync(update);
            }
            ClaimsIdentity identity = new(User.Identity);
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Email));
            identity.AddClaim(new Claim(ClaimTypes.Email, changeEmailModel.Email));
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
    [Authorize]
    [HttpGet("Profile")]
    public async Task<ActionResult<AccountManagementModel>> Profile()
    {
        AccountManagementModel acc = new();
        if (User.Identity.IsAuthenticated)
        {
            string Id = User.FindFirst(ClaimTypes.Sid).Value;
            Query usersRef = db.Collection("Account").WhereEqualTo("Id", Id);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            foreach (DocumentSnapshot VARIABLE in snapshot.Documents)
            {
                acc = VARIABLE.ConvertTo<AccountManagementModel>();
            }
        }
        return await Task.FromResult(acc);
    }
    [Authorize]
    [HttpPost("EditProfile")]
    public async Task<ActionResult> EditProfile([FromBody] AccountManagementModel accountManagementModel)
    {
        try
        {
            QuerySnapshot snapshot = await db.Collection("Account")
                .WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync();
            Dictionary<string, dynamic> update = new()
            {
                { "Name", accountManagementModel.Name },
                { "DateOfBirth", accountManagementModel.DateOfBirth.AddDays(1).ToUniversalTime() }
            };
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                await document.Reference.UpdateAsync(update);
            }

            return Ok("Success");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    [HttpPost("SignUp")]
    public async Task<ActionResult> SignUp([FromBody] SignUpModel signUpModel)
    {
        if (signUpModel.Role == "Customer" || signUpModel.Role == "Studio")
        {

        }
        else
        {
            return BadRequest("Check role");
        }
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
            await docRef.AddAsync(account);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Sid, user.Uid),
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(ClaimTypes.Role, account.Role),
                    new Claim(ClaimTypes.DateOfBirth, account.DateOfBirth.ToString()),
                    new Claim("Token", await user.GetIdTokenAsync())
            }, "serverAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetToken")]
    public async Task<ActionResult<char[]>> GetToken()
    {
        if(User.Identity.IsAuthenticated)
        {
            string token = User.FindFirstValue("Token");
            char[] ch = new char[token.Length];
            for (int i = 0; i < token.Length; i++)
            {
                ch[i] = token[i];
            }
            return await Task.FromResult(ch);
        }
        else
        {
            try
            {
                var uid = "wgiW4ncipWSsHzaHIrHyaD0sSFL2";

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
    [HttpPost("feedback")]
    public async Task<ActionResult> Feedback([FromBody] FeedbackMessageModel feedback)
    {
        await db.Collection("Feedback").AddAsync(feedback);
        return Ok("Done");
    }
    //FIXING
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
                try
                {
                    movies.Add((await db.Collection("Movie").WhereEqualTo("MovieId", view.ElementAt(i).Key).GetSnapshotAsync()).Documents[0].ConvertTo<MovieModel>());
                }
                catch
                {



                }
            }
        }
        else
        {
            foreach (KeyValuePair<string, double> item in view)
            {

                try
                {
                    movies.Add((await db.Collection("Movie").WhereEqualTo("MovieId", item.Key).GetSnapshotAsync()).Documents[0].ConvertTo<MovieModel>());
                }
                catch
                {



                }
            }
        }
        return await Task.FromResult(movies);


    }
    //ENDFIX
}