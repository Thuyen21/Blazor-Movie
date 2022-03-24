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
        public Task<List<MovieViewModel>> GetWithPagingForStudioAsync(int pageSize, int pageIndex, string searchString, string orderBy, string StudioId);
        public Task EditFileAsync(string Id, Stream streamImage, string ImageContentType, Stream streamMovie, string MovieContentType);
    }
}
