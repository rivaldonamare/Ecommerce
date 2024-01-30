using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface ICategoryRepository : IRepositroy<Category>
{
    void Update (Category category);
}
