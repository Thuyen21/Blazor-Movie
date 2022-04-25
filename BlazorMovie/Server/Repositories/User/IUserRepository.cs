using BlazorMovie.Shared;
using BlazorMovie.Shared.Account;

namespace BlazorMovie.Server.Repositories.User
{
    public interface IUserRepository
    {
        public Task<List<UserViewModel>> GetAllAsync();
        public Task<UserViewModel> GetByIdAsync(Guid Id);
        public Task EditAsync(UserViewModel user);
        public Task DeleteAsync(Guid Id);
        //public void DeleteById(Guid Id);
        public Task BanByIdAsync(Guid Id);
        public Task UnBanByIdAsync(Guid Id);
        public Task<List<UserViewModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy);
    }
}
