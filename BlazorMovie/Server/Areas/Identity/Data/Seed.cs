using BlazorMovie.Shared;

namespace BlazorMovie.Server.Areas.Identity.Data
{
    internal static class Seed
    {
        internal static ApplicationRole[] Roles =
        {
            new()
            { 
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Studio",
                NormalizedName = "STUDIO"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            }
        };
    }
}
