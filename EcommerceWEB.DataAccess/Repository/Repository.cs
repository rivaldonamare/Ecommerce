using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.DataAccess.Repository.IRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EcommerceWEB.DataAccess.Repository;

public class Repository<T> : IRepositroy<T> where T : class
{
    private readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        this.dbSet = _context.Set<T>();

    }
    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public IEnumerable<T> GetAll(string includeProperties)
    {
        IQueryable<T> query = dbSet;

        if (includeProperties != null)
        {
            return query.Include(includeProperties).ToList();
        }
        
        return query.ToList();
    }

    public T GetById(Expression<Func<T, bool>> filter, string includeProperties)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);

        if (includeProperties != null)
        {
            return query.Include(includeProperties).SingleOrDefault();
        }
        return query.SingleOrDefault();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }
}
