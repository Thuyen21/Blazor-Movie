using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorMovie.Entities.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Repositories.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext context;
    protected readonly DbSet<TEntity> dbSet;
    private readonly IMapper mapper;

    public BaseRepository(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
        this.mapper = mapper;
    }
    public virtual async Task<IEnumerable<TEntity>> GetAsync(int? page = null, int? pageSize = null,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = dbSet.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        if (page != null)
        {
            query = pageSize != null
                ? query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value)
                : query.Skip((page.Value - 1) * 10).Take(10);
        }
        return await query.ToListAsync();
    }
    public virtual async Task<IEnumerable<TResult>> GetAsync<TResult>(int? page = null,
        int? pageSize = null, Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = dbSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (page != null)
        {
            query = pageSize != null
                ? query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value)
                : query.Skip((page.Value - 1) * 10).Take(10);
        }
        return await query.ProjectTo<TResult>(mapper.ConfigurationProvider).ToListAsync();
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = dbSet.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.CountAsync();
    }
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await dbSet.AddRangeAsync(entities);
    }
    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        dbSet.RemoveRange(entities);
    }
    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        dbSet.UpdateRange(entities);
    }
    public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> filter)
    {
        return await dbSet.Where(filter).ProjectTo<TResult>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }
    public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> filter)
    {
        return await dbSet.Where(filter).ProjectTo<TResult>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await dbSet.Where(filter).FirstOrDefaultAsync();
    }
    public virtual async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await dbSet.Where(filter).SingleOrDefaultAsync();
    }


    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await dbSet.AnyAsync(filter);
    }
    public virtual void Delete(object id)
    {
        TEntity? entityToDelete = dbSet.Find(id);
        if (entityToDelete != null)
        {
            dbSet.Remove(entityToDelete);
        }
    }
    public virtual void Delete(TEntity entityToDelete)
    {
        dbSet.Remove(entityToDelete);
    }
    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Update(entityToUpdate);
    }
    public virtual async Task AddAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }
}

