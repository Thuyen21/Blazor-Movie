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

public class BaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext context;
    protected readonly DbSet<TEntity> dbSet;
    protected readonly IMapper mapper;

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
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await dbSet.AnyAsync(filter);
    }
}
