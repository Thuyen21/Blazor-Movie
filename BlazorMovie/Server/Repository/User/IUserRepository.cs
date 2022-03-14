using BlazorMovie.Shared;

namespace BlazorMovie.Server.Repository.User
{
    public interface IUserRepository
    {
        public List<UserModel> GetAll();
        public UserModel GetById(Guid Id);
        public List<UserModel> Search(string text);
        //public void Add();
        public void Edit(UserModel user);
        //public void Delete(UserModel actor);
        //public void DeleteById(Guid Id);
        public void BanById(Guid Id);
        public void UnBanById(Guid Id);
        public UserModel GetWithPaging(int PageSize, int PageIndex, string SearchString, string OrderBy);

        

    }
}
