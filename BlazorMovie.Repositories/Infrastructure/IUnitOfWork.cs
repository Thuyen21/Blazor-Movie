using BlazorMovie.Repositories.Base;

namespace BlazorMovie.Repositories.Infrastructure;

public interface IUnitOfWork
{
    IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task SaveChangesAsync();
}