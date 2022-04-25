using Microsoft.AspNetCore.Identity;

namespace BlazorMovie.Server.Entity.Data.Account
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
