namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }

    void Save();
}
