using BlazorMovie.Server.Entity.Data.Base;
using BlazorMovie.Shared.Base;
using System.Linq.Expressions;

namespace BlazorMovie.Server.Repositories.Base;

public interface IBaseRepository<InputModel, ViewModel, Data> 
    where InputModel : BaseInputModel, new()
    where ViewModel : BaseViewModel, new()
    where Data : BaseData, new()
{
    public void Add(InputModel inputModel);
    public void Delete(Expression<Func<Data, bool>> expression);
    public ViewModel GetById(Guid id);
    public IEnumerable<Data> Get(Expression<Func<Data, bool>> expression);
    public void Update(Expression<Func<Data, bool>> expression, ViewModel model);
    public List<ViewModel> GetWithPaging(int pageSize, int pageIndex, string searchString, string orderBy);
}
