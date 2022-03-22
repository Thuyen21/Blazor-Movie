using BlazorMovie.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Repositories.Movie
{
    public interface IMovieRepository
    {
        public Task<List<MovieModel>> GetAllAsync();
        public Task<MovieModel> GetByIdAsync(Guid Id);
        public Task Add(MovieModel movie);
        public Task EditAsync(MovieModel movie);
        public Task DeleteByIdAsync(Guid Id);
        public Task<List<MovieViewModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy);
        public string GetMoiveFile(Guid Id);
        public string GetImageFile(Guid Id);
    }
}
