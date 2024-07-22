namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IShoppingCartRepository ShoppingCartRepository { get; }
    IApplicationUserRepository ApplicationUserRepository { get; }
    IOrderHeaderRepository OrderHeaderRepository { get; }
    IOrderDetailsRepository OrderDetailsRepository { get; }

    void Save();
}
