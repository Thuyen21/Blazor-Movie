using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Shared
{
    public class MovieModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public DateTime PremiereDate { get; set; }
        public string? MoviesDescription { get; set; }

        public IFormFile? ImageFile { get; set; }
        public IFormFile? MovieFile { get; set; }

        public Guid StudioId { get; set; }
    }
}
