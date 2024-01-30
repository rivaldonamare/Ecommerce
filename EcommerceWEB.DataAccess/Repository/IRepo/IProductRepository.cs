using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IProductRepository : IRepositroy<Product>
{
    void Update(Product product);
}
