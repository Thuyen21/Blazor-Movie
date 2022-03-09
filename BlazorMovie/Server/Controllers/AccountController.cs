using BlazorMovie.Server.Services;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorMovie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser<Guid>> userManager;

        private readonly SignInManager<IdentityUser<Guid>> signInManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> signInManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender; 
        }
        // Will learn  Email Confirmation later
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!userManager.SupportsUserEmail)
                    {
                        return BadRequest("The default UI requires a user store with email support.");
                    }

                    var result = await userManager.CreateAsync(new IdentityUser<Guid> {UserName = registerModel.Email, Email = registerModel.Email }, registerModel.Password);
                    if (result.Succeeded)
                    {
                        var user = await userManager.FindByNameAsync(registerModel.Email);
                        await userManager.AddToRoleAsync(user, registerModel.Role);
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
        public async Task<IActionResult> ResetPassword([FromBody] string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
               

                await emailSender.SendEmailAsync(
                    email,
                    "Reset Password",
                    $"Code : {code}");
            }
            catch (Exception ex)
            {

            }
           
            return Ok();

        }
        [HttpPost("ResetPasswordConfirmation")]
        public async Task<IActionResult> ResetPasswordConfirmation([FromBody] ResetPasswordConfirmationModel resetPasswordConfirmation)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordConfirmation.Email);
            var result = await userManager.ResetPasswordAsync(user, resetPasswordConfirmation.Code, resetPasswordConfirmation.Password);
            if(result.Succeeded)
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
        public async Task<ActionResult<ClaimsPrincipal>> GetCurrentUser()
        {
            return User;
        }
    }
}
