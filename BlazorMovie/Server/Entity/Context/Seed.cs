using BlazorMovie.Server.Entity.Data.Account;

namespace BlazorMovie.Server.Entity.Context
{
    internal static class Seed
    {
        internal static ApplicationRole[] Roles =
        {
            new()
            {
                Id = new Guid("c294babc-bed5-4402-adc0-d80bf48466ec"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Id = new Guid("cf8c7373-c04f-40a1-b1b7-64612eba45d8"),
                Name = "Studio",
                NormalizedName = "STUDIO"
            },
            new()
            {
                Id = new Guid("d6fceefd-466a-4b02-b748-221c84112a42"),
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            }
        };
        internal static ApplicationUser[] Users =
        {
            new()
            {
                Id = new Guid("219bb40e-0cab-4f08-a408-f33ecb138ed0"),
                UserName = "admin@thuyen.com",
                NormalizedUserName = "admin@thuyen.com".ToUpper(),
                Email = "admin@thuyen.com",
                NormalizedEmail = "admin@thuyen.com".ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                LockoutEnabled = false,
                Wallet = 0,
                DateOfBirth= DateTime.Now,
                Name ="admin@thuyen.com"
            },
            new()
            {
                Id = new Guid("8aacfc8a-3418-46f4-9cf8-395fc5b90499"),
                UserName = "studio@thuyen.com",
                NormalizedUserName = "studio@thuyen.com".ToUpper(),
                Email = "studio@thuyen.com",
                NormalizedEmail = "studio@thuyen.com".ToUpper(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEP00TRDpYIeC8EuQYg8Gts0tf3GYua9guO0irf1F+koiluvei0n/WE8fjg71Bo2TMQ==" ,
                SecurityStamp = string.Empty,
                LockoutEnabled = false,
                Wallet = 0,
                DateOfBirth= DateTime.Now,
                Name ="studio@thuyen.com"
            },
            new()
            {
                Id = new Guid("c37a3f36-08b8-44ba-adda-85f3827811ba"),
                UserName = "customer@thuyen.com",
                NormalizedUserName = "customer@thuyen.com".ToUpper(),
                Email = "customer@thuyen.com",
                NormalizedEmail = "customer@thuyen.com".ToUpper(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEO5DQLS1nAVaqJ1Mx4dWWs8Fwqd7me6LneYfPrdfGUK/Egvme3gYr6Yz481HfWVSDQ==",
                LockoutEnabled = false,
                SecurityStamp = string.Empty,
                Wallet = 0,
                DateOfBirth= DateTime.Now,
                Name ="customer@thuyen.com"
            }

        };
        internal static ApplicationUserRole[] UserRole =
        {
            new()
            {
                UserId = new Guid("219bb40e-0cab-4f08-a408-f33ecb138ed0"),
                RoleId = new Guid("c294babc-bed5-4402-adc0-d80bf48466ec")
            },
            new()
            {
                UserId = new Guid("8aacfc8a-3418-46f4-9cf8-395fc5b90499"),
                RoleId = new Guid("cf8c7373-c04f-40a1-b1b7-64612eba45d8")
            },
            new()
            {
                UserId = new Guid("c37a3f36-08b8-44ba-adda-85f3827811ba"),
                RoleId = new Guid("d6fceefd-466a-4b02-b748-221c84112a42")
            }
        };
    }
}
