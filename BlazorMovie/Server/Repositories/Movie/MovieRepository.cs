using BlazorMovie.Server.Entity.Data;
using BlazorMovie.Server.Services;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovie.Server.Repositories.Movie
{
    public class MovieRepository : IMovieRepository
    {
        private readonly Context context;
        private readonly ILogger logger;
        private readonly FileService fileService;
        public MovieRepository(Context context, ILogger<MovieRepository> logger, FileService fileService)
        {
            this.context = context;
            this.logger = logger;
            this.fileService = fileService;
        }
        public async Task Add(MovieModel movie)
        {
                ApplicationMovie applicationMovie = new ApplicationMovie();
            applicationMovie.Name = movie.Name;
            applicationMovie.Title = movie.Title;
                applicationMovie.PremiereDate = movie.PremiereDate;
                applicationMovie.Description = movie.Description;
                applicationMovie.Genre = movie.Genre;
                applicationMovie.Studio = context.Users.Find(movie.StudioId);
                applicationMovie.Id = Guid.NewGuid();
                if(movie.ImageFile != null && movie.ImageFile.Length > 0)
                {
                    Stream streamImage = new MemoryStream(movie.ImageFile);
                    applicationMovie.ImageFileLink = await fileService.UploadAsync(streamImage, applicationMovie.Id + movie.ImageFileExtensions);
                    streamImage.Close();
                }
            else
            {
                applicationMovie.ImageFileLink = null;
            }

                if (movie.MovieFile != null && movie.MovieFile.Length > 0)
                {
                    Stream streamMovie = new MemoryStream(movie.MovieFile);
                    applicationMovie.MovieFileLink = await fileService.UploadAsync(streamMovie, applicationMovie.Id + movie.MovieFileExtensions);
                    streamMovie.Close();
                }
            else
            {
                applicationMovie.MovieFileLink = null;
            }
            await context.Movies.AddAsync(applicationMovie);
                await context.SaveChangesAsync();
            }
            
        

        public async Task DeleteByIdAsync(Guid Id)
        {
            var movie = context.Movies.Find(Id);
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
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

        public string GetImageFile(Guid Id)
        {
            throw new NotImplementedException();
        }

        public string GetMoiveFile(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MovieViewModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy)
        {
            var query = context.Movies.Include(c => c.Studio).Select(x => new MovieViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Title = x.Title,
                PremiereDate = x.PremiereDate,
                Description = x.Description,
                StudioName = x.Studio.Name,
                Genre = x.Genre,
                MovieFileLink = x.MovieFileLink,
                ImageFileLink = x.ImageFileLink,

            });
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => EF.Functions.Like(x.Name, "%" + searchString + "%"));
            }
            switch (orderBy)
            {
                case "name":
                    query = query.OrderBy(c => c.Name);
                    break;
                case "nameDesc":
                    query = query.OrderByDescending(c => c.Name);
                    break;
                case "date":
                    query = query.OrderBy(c => c.PremiereDate);
                    break;
                case "dateDesc":
                    query = query.OrderByDescending(c => c.PremiereDate);
                    break;
                case "genre":
                    query = query.OrderBy(c => c.Genre);
                    break;
                case "genreDesc":
                    query = query.OrderByDescending(c => c.Genre);
                    break;
            }

            query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var movies = await query.ToListAsync();
            return movies;
        }
    }
}
