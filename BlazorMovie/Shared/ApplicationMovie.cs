using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Shared
{
    public class ApplicationMovie
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public DateTime PremiereDate { get; set; }
        public string? MoviesDescription { get; set; }

        public byte[]? ImageFile { get; set; }
        public byte[]? MovieFile { get; set; }

        public ApplicationUser Studio { get; set; }
    }
}
