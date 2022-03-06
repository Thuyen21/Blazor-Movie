using BlazorMovie.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser<Guid>> UserManager;
       

        public AccountController(UserManager<IdentityUser<Guid>> UserManager)
        {
            this.UserManager = UserManager;
        }
        // Will learn  Email Confirmation later
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!UserManager.SupportsUserEmail)
                    {
                        return BadRequest("The default UI requires a user store with email support.");
                    }

                    var mess = await UserManager.CreateAsync(new IdentityUser<Guid> {UserName = registerModel.Email, Email = registerModel.Email }, registerModel.Password);
                    var user = await UserManager.FindByNameAsync(registerModel.Email);
                    await UserManager.AddToRoleAsync(user, "Admin");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                
            }
            
        }
    }
}
