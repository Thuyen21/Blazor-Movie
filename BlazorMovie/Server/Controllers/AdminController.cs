using BlazorMovie.Server.Repository.User;
using BlazorMovie.Shared;
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
        [HttpGet("UserManagement")]
        public async Task<ActionResult<List<UserModel>>> UserManagement(string? searchString, string? orderBy, int index) => await userRepository.GetWithPagingAsync(20, index, searchString, orderBy);
        [HttpPost("DeleteAccount")]
        public async Task DeleteAccount(Guid Id) => await userRepository.DeleteAsync(Id);
        [HttpPost("EditAccount")]
        public async Task EditAccount(UserModel user) => await userRepository.EditAsync(user);
        [HttpGet("GetUserById")]
        public async Task<UserModel> GetUserByIdAsync(Guid Id) => await userRepository.GetByIdAsync(Id);
    }
}
