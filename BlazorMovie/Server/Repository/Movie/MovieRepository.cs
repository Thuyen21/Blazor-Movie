using BlazorMovie.Server.Data;
using BlazorMovie.Shared;

namespace BlazorMovie.Server.Repository.Movie
{
    public class MovieRepository : IMovieRepository
    {
        private readonly Context context;
        private readonly ILogger logger;
        public MovieRepository( Context context, ILogger<MovieRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public async Task Add(MovieModel movie)
        {
            ApplicationMovie applicationMovie = new ApplicationMovie();
            applicationMovie.Title = movie.Title;
             applicationMovie.PremiereDate = movie.PremiereDate;
            applicationMovie.MoviesDescription = movie.MoviesDescription;
            //applicationMovie.MovieFile = FileHelpers
            using (var writer = new BinaryWriter(movie.ImageFile.OpenReadStream()))
            {
                writer.Write(applicationMovie.ImageFile);
            }
            using (var writer = new BinaryWriter(movie.MovieFile.OpenReadStream()))
            {
                writer.Write(applicationMovie.MovieFile);
            }
            applicationMovie.Studio.Id = movie.StudioId;
            await context.Movies.AddAsync(applicationMovie);
        }

        public Task DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(MovieModel movie)
        {
            throw new NotImplementedException();
        }

        public Task<List<MovieModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MovieModel> GetByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MovieModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy)
        {
            throw new NotImplementedException();
        }
    }
}
