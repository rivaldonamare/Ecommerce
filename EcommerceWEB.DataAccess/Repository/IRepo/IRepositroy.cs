using System.Linq.Expressions;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IRepositroy<T> where T : class
{
    IEnumerable<T> GetAll(string includeProperties);
    T GetById(Expression<Func<T, bool>> filter, string includeProperties);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
