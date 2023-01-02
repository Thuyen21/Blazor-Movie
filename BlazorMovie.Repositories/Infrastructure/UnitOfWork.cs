using AutoMapper;
using BlazorMovie.Entities.Context;
using BlazorMovie.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovie.Repositories.Infrastructure;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    protected readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    public UnitOfWork(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
    public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        return new BaseRepository<TEntity>(context, mapper);
    }
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
