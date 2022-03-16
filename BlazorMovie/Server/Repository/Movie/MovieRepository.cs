using BlazorMovie.Shared;

namespace BlazorMovie.Server.Repository.Movie
{
    public class MovieRepository : IMovieRepository
    {
        public void Add(MovieModel movie)
        {
            ApplicationMovie applicationMovie = new ApplicationMovie();
            applicationMovie.Title = movie.Title;
             applicationMovie.PremiereDate = movie.PremiereDate;
            applicationMovie.MoviesDescription = movie.MoviesDescription;
            //applicationMovie.MovieFile = FileHelpers


        }

        public Task DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(Guid Id)
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
