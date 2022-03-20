﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Shared
{
    public class MovieViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public DateTime PremiereDate { get; set; }
        public string? MoviesDescription { get; set; }
        public string? StudioName { get; set; }
    }
}
