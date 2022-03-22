using BlazorMovie.Shared;

namespace BlazorMovie.Server.Entity.Data
{
    internal static class Seed
    {
        internal static ApplicationRole[] Roles =
        {
            new()
            {
                Id = Guid.Parse("c294babc-bed5-4402-adc0-d80bf48466ec"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Id = Guid.Parse("cf8c7373-c04f-40a1-b1b7-64612eba45d8"),
                Name = "Studio",
                NormalizedName = "STUDIO"
            },
            new()
            {
                Id = Guid.Parse("d6fceefd-466a-4b02-b748-221c84112a42"),
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            }
        };
    }
}
