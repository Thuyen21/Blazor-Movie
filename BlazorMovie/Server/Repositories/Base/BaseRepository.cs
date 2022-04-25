using BlazorMovie.Server.Entity.Context;
using BlazorMovie.Server.Entity.Data.Base;
using BlazorMovie.Server.Extension;
using BlazorMovie.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovie.Server.Repositories.Base;


public abstract class BaseRepository<InputModel, ViewModel, Data> : IBaseRepository<InputModel, ViewModel, Data>
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

    public async Task DeleteAsync(ViewModel model)
    {
        var data = await dbSet.FirstAsync(c => c.Id == model.Id && c.UserId == model.UserId);
        dbSet.Remove(data);
        await context.SaveChangesAsync();
    }

    public async Task<ViewModel> GetByIdAsync(Guid id)
    {
       var data =  dbSet.Where(c => c.Id == id).AsEnumerable().Select(c => c.ConvertTo<ViewModel>()).First();
       return data;
    }

    public async Task UpdateAsync(ViewModel model)
    {
        var data = await dbSet.FirstAsync(c => c.Id == model.Id && c.UserId == model.UserId);
        data = model.ConvertTo<Data>();
        dbSet.Update(data);
        await context.SaveChangesAsync();
    }
}
