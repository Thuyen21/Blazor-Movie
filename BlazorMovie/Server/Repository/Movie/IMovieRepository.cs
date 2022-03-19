using BlazorMovie.Shared;

namespace BlazorMovie.Server.Repository.Movie
{
    public interface IMovieRepository
    {
        public Task<List<MovieModel>> GetAllAsync();
        public Task<MovieModel> GetByIdAsync(Guid Id);
        public Task Add(MovieModel movie);
        public Task EditAsync(MovieModel movie);
        public Task DeleteAsync(Guid Id);
        public Task DeleteById(Guid Id);
        public Task<List<MovieModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy);
    }
}
