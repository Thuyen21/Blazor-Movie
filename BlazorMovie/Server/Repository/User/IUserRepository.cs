using BlazorMovie.Shared;

namespace BlazorMovie.Server.Repository.User
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetAllAsync();
        public Task<UserModel> GetByIdAsync(Guid Id);
        //public void Add();
        public Task EditAsync(UserModel user);
        public Task DeleteAsync(Guid Id);
        //public void DeleteById(Guid Id);
        public Task BanByIdAsync(Guid Id);
        public Task UnBanByIdAsync(Guid Id);
        public Task<List<UserModel>> GetWithPagingAsync(int pageSize, int pageIndex, string searchString, string orderBy);

    }
}
