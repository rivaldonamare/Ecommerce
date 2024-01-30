using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.DataAccess.Repository.IRepo;

namespace EcommerceWEB.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;
    public ICategoryRepository CategoryRepository { get; private set; }
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        CategoryRepository = new CategoryRepository(_context);
    }
   

    public void Save()
    {
        _context.SaveChanges();
    }
}
