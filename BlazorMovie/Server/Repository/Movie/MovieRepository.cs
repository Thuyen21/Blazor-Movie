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
                applicationMovie.MovieFileData = movie.MovieFile;
                applicationMovie.ImageFileData = movie.ImageFile;
                applicationMovie.ImageFileExtensions = movie.ImageFileExtensions;
                applicationMovie.MovieFileExtensions = movie.MovieFileExtensions;

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
        
        public FileStreamResult GetImageFile(Guid Id)
        {
            var file = context.Movies.Where(x => x.Id == Id).Select(x => x.ImageFileData).AsNoTracking().First();

            MemoryStream ms = new MemoryStream();
            ms.Write(file, 0, file.Length);


            return new FileStreamResult(ms, "image/jpeg");
        }

        public IResult GetMoiveFile(Guid Id)
        {
            string path = Path.GetFullPath(Path.Combine($"wwwroot/File/Movie/{Id.ToString()}.mkv"));
            var filestream = System.IO.File.OpenRead(path);
            return Results.File(filestream, contentType: "video/x-matroska", fileDownloadName: Id.ToString(), enableRangeProcessing: true);
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
