using BlazorMovie.Server.Entity.Data.Account;
using BlazorMovie.Server.Entity.Data.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Shared
{
    public class MovieData : BaseData
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public ApplicationUser User { get; set; }
        public string? MovieFileLink { get; set; }
        public string? ImageFileLink { get; set; }
    }
}
