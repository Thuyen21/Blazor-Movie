using BlazorMovie.Server.Entity.Context;
using BlazorMovie.Server.Entity.Data.Base;
using BlazorMovie.Server.Extension;
using BlazorMovie.Shared.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlazorMovie.Server.Repositories.Base;


public abstract class BaseRepository<InputModel, ViewModel, Data, Repository> : IBaseRepository<InputModel, ViewModel, Data>
    where InputModel : BaseInputModel, new()
    where ViewModel : BaseViewModel, new()
    where Data : BaseData, new()
    where Repository : class
{
    protected readonly Context context;
    protected readonly ILogger logger;

    protected BaseRepository(Context context, ILogger<Repository> logger)
    {
        this.logger = logger;
        this.context = context;
    }

    public virtual void Add(InputModel inputModel)
    {
        context.Set<Data>().Add(inputModel.ConvertTo<Data>());
        context.SaveChanges();
    }

    public virtual void Delete(Expression<Func<Data, bool>> expression)
    {
        var data = context.Set<Data>().First(expression);
        context.Set<Data>().Remove(data);
         context.SaveChanges();
    }

    public virtual ViewModel GetById(Guid id)
    {
       var data = context.Set<Data>().Where(c => c.Id == id).AsEnumerable().Select(c => c.ConvertTo<ViewModel>()).First();
       return data;
    }
    public virtual IEnumerable<Data> Get(Expression<Func<Data, bool>> expression)
    {
        return context.Set<Data>().Where(expression).AsEnumerable();
        
    }

    public virtual void Update(Expression<Func<Data, bool>> expression, ViewModel model)
    {
        var data = context.Set<Data>().First(expression);
        data = model.ConvertTo<Data>();
        context.Set<Data>().Update(data);
        context.SaveChanges();
    }
    public List<ViewModel> GetWithPaging(int pageSize, int pageIndex, string searchString, string orderBy)
    {
        var query = context.Movies.Include(c => c.Studio).AsEnumerable();
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, "%" + searchString + "%"));
        }
        switch (orderBy)
        {
            case "name":
                query = query.OrderBy(c => c.Name);
                break;
            case "nameDesc":
                query = query.OrderByDescending(c => c.Name);
                break;
            case "date":
                query = query.OrderBy(c => c.PremiereDate);
                break;
            case "dateDesc":
                query = query.OrderByDescending(c => c.PremiereDate);
                break;
            case "genre":
                query = query.OrderBy(c => c.Genre);
                break;
            case "genreDesc":
                query = query.OrderByDescending(c => c.Genre);
                break;
        }

        query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        var movies = query.Select(c  => c.ConvertTo<ViewModel>()).ToList();
        return movies;
    }
}
