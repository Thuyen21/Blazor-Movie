using BlazorMovie.Server.Repositories.Movie;
using BlazorMovie.Shared.Movie;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IMovieRepository movieRepository;

        public CustomerController(ILogger<CustomerController> logger, IMovieRepository movieRepository)
        {
            this.logger = logger;
            this.movieRepository = movieRepository;
        }
        //[HttpGet("Movie")]
        //public async Task<ActionResult<List<MovieViewModel>>> Movie(string? searchString, string? orderBy, int index)
        //{
        //    try
        //    {
        //        return Ok(await movieRepository.GetWithPagingAsync(20, index, searchString, orderBy));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogWarning(ex, ex.Message);
        //        return Ok(new List<MovieViewModel>());
        //    }
        //}
    }
}
