using BlazorMovie.Shared;
using Microsoft.AspNetCore.Mvc;

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
        public Task<List<MovieViewModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy);
        public IResult GetMoiveFile(Guid Id);
        public IResult GetImageFile(Guid Id);
    }
}
