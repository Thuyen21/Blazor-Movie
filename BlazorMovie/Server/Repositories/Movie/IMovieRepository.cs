using BlazorMovie.Shared.Movie;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMovie.Server.Repositories.Movie
{
    public interface IMovieRepository
    {
        public Task<List<MovieInputModel>> GetAllAsync();
        public Task<MovieInputModel> GetByIdAsync(Guid Id);
        public Task Add(MovieInputModel movie);
        public Task EditAsync(MovieInputModel movie);
        public Task DeleteByIdAsync(Guid Id);
        public Task<List<MovieViewModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy);
        public Task<List<MovieViewModel>> GetWithPagingForStudioAsync(int pageSize, int pageIndex, string searchString, string orderBy, string StudioId);
        public Task EditFileAsync(string Id, Stream streamImage, string ImageContentType, Stream streamMovie, string MovieContentType);
    }
}
