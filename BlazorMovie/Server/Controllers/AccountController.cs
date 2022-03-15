using BlazorMovie.Server.Data;
using BlazorMovie.Server.Repository.User;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IEmailSender emailSender;
        private readonly Context context;
        private readonly IUserRepository userRepository;

        public AccountController(IUserRepository userRepository, Context context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
            this.context = context;
            this.userRepository = userRepository;
        }
        // Will learn  Email Confirmation later
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel.Role == "Customer" || registerModel.Role == "Studio")
            {

            }
            else
            {
                return BadRequest("Error");
            }
            ApplicationUser user = new();
            try
            {
                if (ModelState.IsValid)
                {
                    if (!userManager.SupportsUserEmail)
                    {
                        return BadRequest("The default UI requires a user store with email support.");
                    }

                    var result = await userManager.CreateAsync(new ApplicationUser { UserName = registerModel.Email, Email = registerModel.Email, Wallet = 0, DateOfBirth = registerModel.DateOfBirth, Name = registerModel.Name }, registerModel.Password);
                    if (result.Succeeded)
                    {
                        user = await userManager.FindByNameAsync(registerModel.Email);
                        //await roleManager.CreateAsync(new ApplicationRole() { Name = "Admin" });
                        //await roleManager.CreateAsync(new ApplicationRole() { Name = "Studio" });
                        //await roleManager.CreateAsync(new ApplicationRole() { Name = "Customer" });
                        await userManager.AddToRoleAsync(user, registerModel.Role.ToString());

                    }
                    else
                    {
                        return BadRequest(result.Errors.ElementAt(0).Description);
                    }
                }
                return Ok("Register success");
            }
            catch (Exception ex)
            {
                try
                {
                    await userManager.DeleteAsync(user);
                }
                catch
                {

                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            var result = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: loginModel.RememberMe, false);
            if (result.Succeeded)
            {
                return Ok("User logged in.");
            }
            else
            {
                return BadRequest("Invalid login attempt.");
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok("User logged out.");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailModel email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email.Email);
                var user = await userManager.FindByEmailAsync(email.Email);
                var code = await userManager.GeneratePasswordResetTokenAsync(user);


                await emailSender.SendEmailAsync(
                    email.Email,
                    "Reset Password",
                    $"Code : {code}");

                return Ok("A mail had sent");
            }
            catch (Exception ex)
            {
                return BadRequest("Error");
            }
        }

        [HttpPost("ResetPasswordConfirmation")]
        public async Task<IActionResult> ResetPasswordConfirmation([FromBody] ResetPasswordConfirmationModel resetPasswordConfirmation)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordConfirmation.Email);
            var result = await userManager.ResetPasswordAsync(user, resetPasswordConfirmation.Code, resetPasswordConfirmation.Password);
            if (result.Succeeded)
            {
                return Ok("Succeeded");
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost("GetCurrentUser")]
        public async Task<ActionResult<UserModel>> GetCurrentUser()
        {
            try
            {
                UserModel user = await userRepository.GetById(new Guid(userManager.GetUserId(User)));
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpPost("ChangeEmail")]
        public async Task<ActionResult<string>> ChangeEmail([FromBody] ChangeEmailModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var checkPass = await userManager.CheckPasswordAsync(user, model.Password);
            if (checkPass)
            {
                try
                {
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email.ToUpper();
                    user.UserName = model.Email;
                    user.NormalizedUserName = model.Email.ToUpper();
                    await userManager.UpdateAsync(user);
                    return Ok("Success");
                }
                catch (Exception ex)
                {


                    return BadRequest("Error");
                }
            }
            return BadRequest("Error password");
        }

        [Authorize]
        [HttpPost("UpdateProfile")]
        public async Task<ActionResult<string>> UpdateProfile([FromBody] UserModel model)
        {
            var user = await userManager.GetUserAsync(User);
            try
            {
                user.Name = model.Email;
                user.DateOfBirth = model.DateOfBirth.Value;
                await userManager.UpdateAsync(user);
                return Ok("Updated");
            }
            catch (Exception ex)
            {
                return BadRequest("Error");
            }
        }
    }
}
