using BlazorMovie.Server.Repository.Movie;
using BlazorMovie.Server.Repository.User;
using BlazorMovie.Server.Services;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger logger;
        private readonly IMovieRepository movieRepository;
        
        public AdminController(ILogger<AdminController> logger, IUserRepository userRepository, IMovieRepository movieRepository)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.movieRepository = movieRepository;
        }
        [HttpGet("UserManagement")]
        public async Task<ActionResult<List<UserModel>>> UserManagement(string? searchString, string? orderBy, int index)
        {
            try
            {
                return Ok(await userRepository.GetWithPagingAsync(20, index, searchString, orderBy));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
                return Ok(new List<UserModel>());
            }
           
        }

        [HttpPost("DeleteAccount")]
        public async Task<ActionResult> DeleteAccount(Guid Id)
        {
            try
            {
                await userRepository.DeleteAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
                return BadRequest("Error");
            } 
        }
        [HttpPost("EditAccount")]
        public async Task<ActionResult> EditAccount(UserModel user)
        {
            
            try
            {
                await userRepository.EditAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
                return BadRequest("Error");
            }
        }
        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserModel>> GetUserByIdAsync(Guid Id)
        {

            try
            {
                return Ok(await userRepository.GetByIdAsync(Id));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
                return BadRequest("Error");
            }
        }
        [HttpPost("Ban")]
        public async Task<ActionResult> Ban([FromBody] Guid Id)
        {
            try
            {
                await userRepository.BanByIdAsync(Id);
                return Ok("Banned");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
                return BadRequest("Error");
            }
        }
        [HttpPost("UnBan")]
        public async Task<ActionResult> UnBan([FromBody] Guid Id)
        {
            try
            {
                await userRepository.UnBanByIdAsync(Id);
                return Ok("UnBanned");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
                return BadRequest("Error");
            }
        }

        [HttpPost("MovieUpload")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> MovieUpload([FromBody] MovieModel movie)
        {
            movie.StudioId = Guid.NewGuid();
            movie.Id = Guid.NewGuid();
            await movieRepository.Add(movie);
            return Ok();
        }
        [HttpGet("Movie")]
        public async Task<ActionResult<List<MovieViewModel>>> Movie(string? searchString, string? orderBy, int index)
        {
            try
            {
                return Ok(await movieRepository.GetWithPagingAsync(20, index, searchString, orderBy));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
                return Ok(new List<MovieViewModel>());
            }
        }
    }
}
