using BlazorMovie.Shared;

namespace BlazorMovie.Server.Repository.User
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetAll();
        public Task<UserModel> GetById(Guid Id);
        //public void Add();
        public Task Edit(UserModel user);
        //public void Delete(UserModel actor);
        //public void DeleteById(Guid Id);
        public Task BanById(Guid Id);
        public Task UnBanById(Guid Id);
        public Task<List<UserModel>> GetWithPaging(int pageSize, int pageIndex, string searchString, string orderBy);

    }
}
