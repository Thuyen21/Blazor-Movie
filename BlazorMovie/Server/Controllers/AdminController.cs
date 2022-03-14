using BlazorMovie.Server.Repository.User;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        [HttpGet("Get")]
        public async Task<ActionResult<List<UserModel>>> Get()
        {
            return userRepository.GetAll();
        }
    }
}
