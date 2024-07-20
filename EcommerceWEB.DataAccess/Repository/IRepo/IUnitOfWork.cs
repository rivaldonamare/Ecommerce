namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    ICompanyRepository CompanyRepository { get; }

    void Save();
}
