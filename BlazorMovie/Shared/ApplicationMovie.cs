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
        public string? Genre { get; set; }
        public ApplicationUser? Studio { get; set; }
        public string? MovieFileName { get; set; }
        public string? MovieFileExtensions { get; set; }
        public byte[] MovieFileData { get; set; }
        public string? ImageFileExtensions { get; set; }
        public byte[] ImageFileData { get; set; }
        public string? ImageFileName { get; set; }

    }
}
