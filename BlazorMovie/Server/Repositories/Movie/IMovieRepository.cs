using BlazorMovie.Server.Repositories.Base;
using BlazorMovie.Shared;
using BlazorMovie.Shared.Movie;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Repositories.Movie
{
    public interface IMovieRepository : IBaseRepository<MovieInputModel, MovieViewModel, MovieData>
    {
        
    }
}
