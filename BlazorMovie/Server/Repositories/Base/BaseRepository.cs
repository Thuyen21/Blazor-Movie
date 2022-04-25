using BlazorMovie.Server.Entity.Context;
using BlazorMovie.Server.Entity.Data.Base;
using BlazorMovie.Server.Extension;
using BlazorMovie.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovie.Server.Repositories.Base;


public class BaseRepository<InputModel, ViewModel, Data> : IBaseRepository<InputModel, ViewModel, Data>
    where InputModel : BaseInputModel, new()
    where ViewModel : BaseViewModel, new()
    where Data : BaseData, new()
{
    private readonly DbSet<Data> dbSet;
    
    protected readonly Context context;
    public BaseRepository(Context context)
    {
        this.context = context;
    }

    public async Task AddAsync(InputModel inputModel)
    {
       await dbSet.AddAsync(inputModel.ConvertTo<Data>());
       await context.SaveChangesAsync();
    }

    public Task DeleteAsync(ViewModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ViewModel> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ViewModel model)
    {
        throw new NotImplementedException();
    }
}
