using BlazorMovie.Server.Entity.Context;
using BlazorMovie.Server.Repositories.Base;
using BlazorMovie.Server.Services;
using BlazorMovie.Shared;
using BlazorMovie.Shared.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovie.Server.Repositories.Movie
{
    public class MovieRepository : BaseRepository<MovieInputModel, MovieViewModel, MovieData, MovieRepository>, IMovieRepository
    {
        private readonly FileService fileService;
        public MovieRepository(Context context, ILogger<MovieRepository> logger, FileService fileService) : base(context, logger)
        {
            this.fileService = fileService;
        }
        public override void Add(MovieInputModel movie)
        {
            MovieData movieData = new();
            if (movie.ImageFile != null && movie.ImageFile.Length > 0)
            {
                Stream streamImage = new MemoryStream(movie.ImageFile);
                movieData.ImageFileLink = fileService.UploadAsync(streamImage, movieData.Id + movie.ImageFileExtensions).Result;
                streamImage.Close();
            }
            else
            {
                movieData.ImageFileLink = null;
            }

            if (movie.MovieFile != null && movie.MovieFile.Length > 0)
            {
                Stream streamMovie = new MemoryStream(movie.MovieFile);
                movieData.MovieFileLink = fileService.UploadAsync(streamMovie, movieData.Id + movie.MovieFileExtensions).Result;
                streamMovie.Close();
            }
            else
            {
                movieData.MovieFileLink = null;
            }
             context.Movies.Add(movieData);
             context.SaveChanges();
        }            
    
    }
}
