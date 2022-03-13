using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Shared
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTime DateOfBirth { get; set; }
        public double Wallet { get; set; } = 0;
        public string? UserAgent { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}
