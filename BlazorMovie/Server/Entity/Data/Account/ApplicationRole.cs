using Microsoft.AspNetCore.Identity;

namespace BlazorMovie.Server.Entity.Data.Account
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
