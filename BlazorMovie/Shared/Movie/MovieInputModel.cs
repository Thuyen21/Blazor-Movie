using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BlazorMovie.Shared.Base;

namespace BlazorMovie.Shared.Movie
{
    public class MovieInputModel : BaseInputModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Description { get; set; }
        public byte[]? ImageFile { get; set; }
        public byte[]? MovieFile { get; set; }
        public string? ImageFileExtensions { get; set; }
        public string? MovieFileExtensions { get; set; }
    }
}
