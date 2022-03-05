using BlazorMovie.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Enity.Data
{
    public class Context : IdentityDbContext
    {
        public DbSet<AccountManagementModel> AccountManagementModels { get; set; }
        public DbSet<MovieModel> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("workstation id=CrtDatabase.mssql.somee.com;packet size=4096;user id=crt112233_SQLLogin_1;pwd=s7m5dc5lpj;data source=CrtDatabase.mssql.somee.com;persist security info=False;initial catalog=CrtDatabase");
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-UO4APTR\SQLEXPRESS;Database=myDataBase;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
