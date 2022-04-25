using BlazorMovie.Server.Repositories.Movie;
using BlazorMovie.Shared.Movie;
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
        public async Task<ActionResult> MovieUpload([FromBody] MovieInputModel movie)
        {
            try
            {
                movie.StudioId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                movieRepository.Add(movie);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error");
            }

        }
        //[HttpGet("Movie")]
        //public async Task<ActionResult<List<MovieViewModel>>> Movie(string? searchString, string? orderBy, int index)
        //{
        //    try
        //    {
        //        return Ok(await movieRepository.GetWithPagingForStudio(20, index, searchString, orderBy, User.FindFirstValue(ClaimTypes.NameIdentifier)));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogWarning(ex, ex.Message);
        //        return Ok(new List<MovieViewModel>());
        //    }
        //}
        //[HttpPost("MovieUploadForWeb")]
        //public async Task MovieUploadForWeb(string Id, IFormFile ImageFileUp, IFormFile MovieFileUp)
        //{
        //    try
        //    {
        //        await movieRepository.EditFileAsync(Id, ImageFileUp.OpenReadStream(), ImageFileUp.ContentType, MovieFileUp.OpenReadStream(), MovieFileUp.ContentType);
        //     }
        //    catch (Exception ex)
        //    {
        //        logger.LogWarning(ex, ex.Message);
                
        //    }
        //}
    }
}
