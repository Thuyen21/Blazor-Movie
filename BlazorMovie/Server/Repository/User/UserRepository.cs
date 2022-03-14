using BlazorMovie.Server.Data;
using BlazorMovie.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovie.Server.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly Context context;
        public UserRepository(UserManager<ApplicationUser> userManager, Context context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public async void BanById(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());
            user.LockoutEnabled = true;
           await userManager.UpdateAsync(user);
        }
        public async void UnBanById(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());
            user.LockoutEnabled = false;
            await userManager.UpdateAsync(user);
        }
        public async void Edit(UserModel userModel)
        {
            var user = await userManager.FindByIdAsync(userModel.Id.ToString());
            user.DateOfBirth = userModel.DateOfBirth.Value;

            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles.ToArray());
            await userManager.AddToRoleAsync(user, userModel.Role.ToString());
            user.Name = userModel.Name; 
            await userManager.UpdateAsync(user);
        }

        public List<UserModel> GetAll()
        {

           var user = context.Users.Include(c => c.UserRoles).ThenInclude(c => c.Role).Select(c => 
            
                new ApplicationUser()
                {
                        Id = c.Id,
                        Name = c.Name,
                        Email = c.Email,
                        DateOfBirth = c.DateOfBirth,
                        Wallet = c.Wallet,
                        UserRoles = c.UserRoles,
                        UserAgent = c.UserAgent,
               
            }
                
            ).ToList();

            List<UserModel> list = new();
            foreach(var item in user)
            {
                list.Add(new UserModel() { DateOfBirth = item.DateOfBirth, Email = item.Email, Id = item.Id, Name = item.Name, Role= (RoleEnum)Enum.Parse(typeof(RoleEnum), item.UserRoles.ToList()[0].Role.Name), UserAgent= item.UserAgent, Wallet= item.Wallet });
            }

            return list;

        }
        public UserModel GetById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetWithPaging(int PageSize, int PageIndex, string SearchString, string OrderBy)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> Search(string text)
        {
            throw new NotImplementedException();
        }
    }
}
