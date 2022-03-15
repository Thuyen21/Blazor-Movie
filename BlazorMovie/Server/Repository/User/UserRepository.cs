using BlazorMovie.Server.Data;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovie.Server.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger logger;

        private readonly Context context;
        public UserRepository(UserManager<ApplicationUser> userManager, Context context, ILogger<UserRepository> logger)
        {
            this.userManager = userManager;
            this.context = context;
            this.logger = logger;
        }
        public async Task BanByIdAsync(Guid Id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(Id.ToString());
                user.LockoutEnabled = true;
                await userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
            }
            
        }
        public async Task UnBanByIdAsync(Guid Id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(Id.ToString());
                user.LockoutEnabled = false;
                await userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);
            }
        }
        public async Task EditAsync(UserModel userModel)
        {
            var user = await userManager.FindByIdAsync(userModel.Id.ToString());
            user.DateOfBirth = userModel.DateOfBirth.Value;

            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles.ToArray());
            await userManager.AddToRoleAsync(user, userModel.Role.ToString());
            user.Name = userModel.Name;
            await userManager.UpdateAsync(user);
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            
            var user = await context.Users.Include(c => c.UserRoles).ThenInclude(c => c.Role).Select(c =>

                 new UserModel()
                 {
                     Id = c.Id,
                     Name = c.Name,
                     Email = c.Email,
                     DateOfBirth = c.DateOfBirth,
                     Wallet = c.Wallet,
                     Role = c.UserRoles.ToList()[0].Role.Name,
                     UserAgent = c.UserAgent,
                 }

             ).ToListAsync();
            return user;

        }
        public async Task<UserModel> GetByIdAsync(Guid Id)
        {
            var user = await context.Users.Include(c => c.UserRoles).ThenInclude(c => c.Role).Where(c => c.Id == Id).Select(c =>

                new UserModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    Wallet = c.Wallet,
                    Role = c.UserRoles.ToList()[0].Role.Name,
                    UserAgent = c.UserAgent,
                }
            ).FirstAsync();

            return user;
        }

        public async Task<List<UserModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy)
        {
            try
            {
                var query = context.Users.Include(c => c.UserRoles).ThenInclude(c => c.Role).Select(c =>

                new UserModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    DateOfBirth = c.DateOfBirth,
                    Wallet = c.Wallet,
                    Role = c.UserRoles.ToList()[0].Role.Name,
                    UserAgent = c.UserAgent,
                }
            );

                if (!String.IsNullOrEmpty(searchString))
                {
                    query = query.Where(x => EF.Functions.Like(x.Email, "%"+searchString+"%"));
                }
                switch (orderBy)
                {
                    case "name":
                        query = query.OrderBy(c => c.Name);
                        break;
                    case "nameDesc":
                        query = query.OrderByDescending(c => c.Name);
                        break;
                    case "date":
                        query = query.OrderBy(c => c.DateOfBirth);
                        break;
                    case "dateDesc":
                        query = query.OrderByDescending(c => c.DateOfBirth);
                        break;
                    case "email":
                        query = query.OrderBy(c => c.Email);
                        break;
                    case "emailDesc":
                        query = query.OrderByDescending(c => c.Email);
                        break;
                }

                query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                var user = await query.ToListAsync();
                return user;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task DeleteAsync(Guid Id)
        {
            await userManager.DeleteAsync(new ApplicationUser() { Id = Id });
        }
    }
}
