using BlazorMovie.Server.Entity.Data.Base;
using BlazorMovie.Shared.Base;

namespace BlazorMovie.Server.Repositories.Base;

public interface IBaseRepository<InputModel, ViewModel, Data> 
    where InputModel : BaseInputModel, new()
    where ViewModel : BaseViewModel, new()
    where Data : BaseData, new()
{
    public Task AddAsync(InputModel inputModel);
    public Task UpdateAsync(ViewModel model);
    public Task DeleteAsync(ViewModel model);
    public Task<ViewModel> GetByIdAsync(Guid id);

}
