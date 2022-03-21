using BlazorMovie.Server.Data;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                ApplicationMovie applicationMovie = new ApplicationMovie();
                applicationMovie.Title = movie.Title;
                applicationMovie.PremiereDate = movie.PremiereDate;
                applicationMovie.MoviesDescription = movie.MoviesDescription;
                applicationMovie.Studio = context.Users.Find(movie.StudioId);
                applicationMovie.Id = Guid.NewGuid();
                

                await context.Movies.AddAsync(applicationMovie);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
            var query = context.Movies.Select(x => new MovieViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Title = x.Title,
                PremiereDate = x.PremiereDate,
                MoviesDescription = x.MoviesDescription,
                StudioName = x.Studio.Name,
                Genre = x.Genre,
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
