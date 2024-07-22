using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.DataAccess.Repository.IRepo;

namespace EcommerceWEB.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;
    public ICategoryRepository CategoryRepository { get; private set; }
    public IProductRepository ProductRepository { get; private set; }
    public ICompanyRepository CompanyRepository { get; private set; }
    public IShoppingCartRepository ShoppingCartRepository { get; private set; }
    public IApplicationUserRepository ApplicationUserRepository { get; private set; }
    public IOrderHeaderRepository OrderHeaderRepository { get; private set; }
    public IOrderDetailsRepository OrderDetailsRepository { get; private set; }
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        CategoryRepository = new CategoryRepository(_context);
        ProductRepository = new ProductRepository(_context);
        CompanyRepository = new CompanyRepository(_context);
        ShoppingCartRepository = new ShoppingCartRepository(_context);
        ApplicationUserRepository = new ApplicationUserRepository(_context);
        OrderHeaderRepository = new OrderHeaderRepository(_context);
        OrderDetailsRepository = new OrderDetailsRepository(_context);
    }
   

    public void Save()
    {
        _context.SaveChanges();
    }
}
