using BlazorMovie.Server.Repositories.Movie;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorMovie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IMovieRepository movieRepository;

        public StudioController(ILogger<StudioController> logger, IMovieRepository movieRepository)
        {
            this.logger = logger;
            this.movieRepository = movieRepository;
        }
        [HttpPost("MovieUpload")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> MovieUpload([FromBody] MovieModel movie)
        {
            try
            {
                movie.StudioId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await movieRepository.Add(movie);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error");
            }

        }
    }
}
