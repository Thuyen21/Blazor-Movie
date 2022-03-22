using BlazorMovie.Server.Options;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorMovie.Server.Entity.Data;

public class Context : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<ApplicationMovie> Movies { get; set; }
    private SuperUser Options { get; }
    private ApplicationUser[] users { get; } = Seed.Users;
    public Context(DbContextOptions<Context> options, IOptions<SuperUser> optionsAccessor)
        : base(options)
    {
        Options = optionsAccessor.Value;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<ApplicationRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            b.HasData(Seed.Roles);
        });
        var hasher = new PasswordHasher<IdentityUser>();
        
        users[0].PasswordHash = hasher.HashPassword(null, Options.Password);
        users[1].PasswordHash = hasher.HashPassword(null, Options.Password);
        users[2].PasswordHash = hasher.HashPassword(null, Options.Password);
        builder.Entity<ApplicationUser>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            b.HasData(users);
        });

        builder.Entity<ApplicationUserRole>(b =>
        {
            b.HasData(Seed.UserRole);
        });
    }
}
