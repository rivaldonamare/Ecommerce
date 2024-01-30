namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }

    void Save();
}
