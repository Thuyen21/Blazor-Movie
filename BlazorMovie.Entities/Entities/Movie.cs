using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Entities.Entities;


public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Trailer { get; set; }
    public string InTheaters { get; set; }
    public string Poster { get; set; }
    public DateTime ReleaseDate { get; set; }
}


