﻿using Microsoft.AspNetCore.Http;
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

namespace BlazorMovie.Shared
{
    public class MovieModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public DateTime PremiereDate { get; set; }
        public string? MoviesDescription { get; set; }
        public byte[]? ImageFile { get; set; }
        public byte[]? MovieFile { get; set; }

        public Guid StudioId { get; set; }

        
    }
}
