using BlazorApp3.Shared;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BlazorApp3.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

       
        private static readonly FirebaseAuthConfig config = new()
        {
            ApiKey = "AIzaSyAqCxl98i68Te5_xy3vgMcAEoF5qiBKE9o",
            AuthDomain = "movie2-e3c7b.firebaseapp.com",
            Providers = new FirebaseAuthProvider[] {
                // Add and configure individual providers

                new EmailProvider()

                // ...
            }
        };

        private static readonly FirebaseAuthClient client = new(config);

        private static UserCredential userCredential;

        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] LogInModel logIn)
        {

            try
            {
                userCredential = await client.SignInWithEmailAndPasswordAsync(logIn.Email, logIn.Password);
                
            }
            catch (FirebaseAuthHttpException ex)
            {
                return BadRequest(ex.Reason.ToString());
            }

            User user = userCredential.User;
            FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
            Query usersRef = db.Collection("Account").WhereEqualTo("Id", user.Uid);
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            AccountManagementModel acc = new();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                acc = document.ConvertTo<AccountManagementModel>();
            }
            
            
            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, logIn.Email),
                    new Claim(ClaimTypes.Sid, user.Uid),
                    new Claim(ClaimTypes.Name, acc.Name),
                    new Claim(ClaimTypes.Role, acc.Role),
                    new Claim(ClaimTypes.DateOfBirth, acc.DateOfBirth.ToShortDateString()),
                    new Claim("Token", await user.GetIdTokenAsync(true))
                   
            }, "serverAuth");
            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            //Sign In User
            await HttpContext.SignInAsync(claimsPrincipal);

            return Ok();
        }
        
        [HttpGet("Logout")]
        
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
        
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<AccountManagementModel>> GetCurrentUser()
        {
            //var logInModel = new LogInModel();
            //if (User.Identity.IsAuthenticated) logInModel.Email = User.FindFirstValue(ClaimTypes.Name);
            //return await Task.FromResult(logInModel);
            AccountManagementModel acc = new();
            if(User.Identity.IsAuthenticated)
            {
                string Id = User.FindFirst(ClaimTypes.Sid).Value;
                FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");

                Query usersRef = db.Collection("Account").WhereEqualTo("Id", Id);

                QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();


                foreach (DocumentSnapshot VARIABLE in snapshot.Documents)
                {
                    acc = VARIABLE.ConvertTo<AccountManagementModel>();
                }
            }
            
            return await Task.FromResult(acc);

        }
        
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordModel resetPassword)
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
        public async Task<ActionResult<AccountManagementModel>> Profile ()
        {
            AccountManagementModel acc = new();
            if (User.Identity.IsAuthenticated)
            {
                string Id = User.FindFirst(ClaimTypes.Sid).Value;
                FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");

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
                FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
                QuerySnapshot snapshot = await db.Collection("Account")
                    .WhereEqualTo("Id", User.FindFirstValue(ClaimTypes.Sid)).GetSnapshotAsync();
                Dictionary<string, object> update = new()
                {
                    { "Name", accountManagementModel.Name },
                    { "DateOfBirth", accountManagementModel.DateOfBirth.ToUniversalTime() }
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
            if (signUpModel.ConfirmPassword != signUpModel.Password)
            {
                return BadRequest("Password and Confirm Password are different");
            }
            signUpModel.Email = signUpModel.Email.ToLower();
            try
            {
                //var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyA1sc-XyNBvPFAs3ZwkcU6BBV9vbsJrUL0"));
                //var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(signUp.Email,signUp.Password,signUp.Name);

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
                FirestoreDb db = FirestoreDb.Create("movie2-e3c7b");
                CollectionReference docRef = db.Collection("Account");

                AccountManagementModel account = new()
                {
                    DateOfBirth = signUpModel.DateOfBirth.ToUniversalTime(),
                    Email = signUpModel.Email,
                    Id = user.Uid,
                    Name = signUpModel.Name,
                    Role = "Customer",
                    Wallet = 0.0
                };


                await docRef.AddAsync(account);
                var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Sid, user.Uid),
                    new Claim(ClaimTypes.Name, account.Name),
                    new Claim(ClaimTypes.Role, account.Role),
                    new Claim(ClaimTypes.DateOfBirth, account.DateOfBirth.ToString()),
                    new Claim("Token", await user.GetIdTokenAsync())
            }, "serverAuth");
                //create claimsPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                //Sign In User
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
            
            var token = User.FindFirstValue("Token");
            char[] ch = new char[token.Length];
            for (int i = 0; i < token.Length; i++)
            {
                ch[i] = token[i];
            }
            return await Task.FromResult(ch);
        }
    }
}