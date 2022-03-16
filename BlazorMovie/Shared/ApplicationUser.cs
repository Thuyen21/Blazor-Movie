using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovie.Shared
{
    [Index(nameof(Email))]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Wallet { get; set; } = 0;
        public string? UserAgent { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }

        public ICollection<ApplicationMovie> Movies { get; set; }
    }
}
