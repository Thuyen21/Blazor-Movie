using System.Linq.Expressions;

namespace BlazorMovie.Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetAsync(int? page = null, int? pageSize = null, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<IEnumerable<TResult>> GetAsync<TResult>(int? page = null, int? pageSize = null, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        Task<TResult?> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> filter);
        void Update(TEntity entityToUpdate);
        void UpdateRange(IEnumerable<TEntity> entities);
    }
}